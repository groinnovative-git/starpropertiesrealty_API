using Microsoft.IdentityModel.Tokens;
using Star_Properties.BAL.Interface.IAuthBAL;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.IAuthRepository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Star_Properties.BAL.Service.AuthBAL
{
    public class AuthBAL : IAuthBAL
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthBAL(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _repo.GetUserByUsername(request.Username);

            if (user == null)
                throw new Exception("Invalid credentials");

            // Hash input password
            var hashedInput = HashPassword(request.Password);

            // Compare with DB hash
            if (hashedInput != user.Password)
                throw new Exception("Invalid credentials");

            var token = GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            };
        }

        private string GenerateToken(UserMaster user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_config["JwtSettings:ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public async Task<string> CreateUserCrediential(CreateUserRequest request,Guid userId)
        {
            var existingEmailUser = await _repo.GetUserByEmail(request.Email);
            var existingUsername = await _repo.GetUserByUsername(request.Username);

            if (existingUsername != null)
                throw new Exception("User already exists");
            if (existingEmailUser != null)
                throw new Exception("User already exists");

            var user = new UserMaster
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Name = request.Name,
                Email = request.Email,
                Password = HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            await _repo.CreateUserCrediential(user);

            return "User created successfully";
        }

        public async Task<string> UpdateUserCrediential(UpdateUserRequest request, Guid userId)
        {
            var user = await _repo.GetUserByUserId(request.UserId);
            var username = await _repo.GetUserByUsername(request.Username);

            if (user == null)
                throw new Exception("User not found");

            if (username != null)
                throw new Exception("User already exists");

            var requestedUsername = request.Username?.Trim();
            var requestedEmail = request.Email?.Trim();
            var requestedName = request.Name?.Trim();
            var requestedRole = request.Role?.Trim();

            return "User updated successfully";
        }

        public async Task<string> DeleteUserCrediential(DeleteUserCredientialsRequest request, Guid userId)
        {
            if (request == null || request.UserId == Guid.Empty)
                return "Invalid Request";

            var result = await _repo.DeleteUserCrediential(request.UserId, userId);

            return result;
        }

        public async Task<List<UserMaster>> GetUserCrediential()
        {
            var result = await _repo.GetUserCrediential();
            return result;
        }
    }
}
