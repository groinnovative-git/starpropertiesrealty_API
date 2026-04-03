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
            var existingUser = await _repo.GetUserByUsername(request.Username);
            var existingEmailUser = await _repo.GetUserByEmail(request.Email);

            if (existingUser != null)
                throw new Exception("User already exists");
            if (existingEmailUser != null)
                throw new Exception("Email already exists");

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

            if (user == null)
                throw new Exception("User not found");

            var requestedUsername = request.Username?.Trim();
            var requestedEmail = request.Email?.Trim();
            var requestedName = request.Name?.Trim();
            var requestedRole = request.Role?.Trim();

            if (string.IsNullOrWhiteSpace(requestedUsername))
                throw new Exception("Username is required");
            if (string.IsNullOrWhiteSpace(requestedEmail))
                throw new Exception("Email is required");
            if (string.IsNullOrWhiteSpace(requestedName))
                throw new Exception("Name is required");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");
            if (string.IsNullOrWhiteSpace(requestedRole))
                throw new Exception("Role is required");

            var skippedFields = new List<string>();

            if (!string.Equals(user.Username, requestedUsername, StringComparison.OrdinalIgnoreCase))
            {
                var existingUserByUsername = await _repo.GetUserByUsername(requestedUsername);
                if (existingUserByUsername != null && existingUserByUsername.UserId != request.UserId)
                {
                    skippedFields.Add("username");
                    requestedUsername = user.Username;
                }
            }

            if (!string.Equals(user.Email, requestedEmail, StringComparison.OrdinalIgnoreCase))
            {
                var existingUserByEmail = await _repo.GetUserByEmail(requestedEmail);
                if (existingUserByEmail != null && existingUserByEmail.UserId != request.UserId)
                {
                    skippedFields.Add("email");
                    requestedEmail = user.Email;
                }
            }

            user.Username = requestedUsername;
            user.Email = requestedEmail;
            user.Name = requestedName;
            user.Password = HashPassword(request.Password);
            user.Role = requestedRole;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = userId;

            await _repo.UpdateUserCrediential(user);

            if (skippedFields.Any())
                return $"User updated successfully. Skipped duplicate field(s): {string.Join(", ", skippedFields)}";

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
