using Microsoft.IdentityModel.Tokens;
using Star_Properties.BAL.Interface.IAuthBAL;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.IAuthRepository;
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
            var user = await _repo.GetUserByEmailAsync(request.Email);

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
                Email = user.Email,
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
                new Claim(ClaimTypes.Email, user.Email),
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
    }
}
