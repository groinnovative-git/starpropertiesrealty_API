using Microsoft.AspNetCore.Mvc;
using Star_Properties.BAL.Interface.IAuthBAL;
using Star_Properties.Model.RequestModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star_Properties.Controllers.Admin.AuthController
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBAL _authBAL;

        public AuthController(IAuthBAL authBAL)
        {
            _authBAL = authBAL;
        }

        private Guid GetUserIdFromToken()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    throw new UnauthorizedAccessException("Invalid token: UserId not found");

                return Guid.Parse(userIdClaim);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error extracting userId from token: {ex.Message}", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _authBAL.Login(request);
                return Ok(result);
            }
            catch
            {
                return Unauthorized("Invalid username or password");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserCrediential(CreateUserRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();

                var result = await _authBAL.CreateUserCrediential(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserCrediential(UpdateUserRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _authBAL.UpdateUserCrediential(request,userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
