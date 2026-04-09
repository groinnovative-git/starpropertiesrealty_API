using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Star_Properties.BAL.Interface.IAuthBAL;
using Star_Properties.BAL.Interface.IEmailBAL;
using Star_Properties.Model.RequestModel;
using Star_Properties.Repository.Interface.IAuthRepository;

namespace Star_Properties.Controllers.Admin.AuthController
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBAL _authBAL;
        private readonly IEmailBAL _emailBAL;
        private readonly IAuthRepository _authRepository;
        private readonly IEmailQueueBAL _emailQueueBAL;

        public AuthController(
            IAuthBAL authBAL,
            IEmailBAL emailBAL,
            IAuthRepository authRepository,
            IEmailQueueBAL emailQueueBAL)
        {
            _authBAL = authBAL;
            _emailBAL = emailBAL;
            _authRepository = authRepository;
            _emailQueueBAL = emailQueueBAL;
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

        [AllowAnonymous]
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
                var responseMessage = result;

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var createdByUser = await _authRepository.GetUserByUserId(userId);

                    var emailRequest = new SendEmailRequest
                    {
                        ToEmails = new List<string> { request.Email },
                        TemplateType = "CreateCredential",
                        CompanyName = "StarPropertiesAndRealty",
                        LoginUrl = string.Empty,
                        UserName = request.Name,
                        Username = request.Username,
                        Email = request.Email,
                        Role = request.Role,
                        Password = request.Password,
                        CreatedByName = createdByUser?.Name ?? createdByUser?.Username ?? "Admin",
                        CreatedByEmail = createdByUser?.Email ?? string.Empty,
                        CreatedDateTime = DateTime.UtcNow
                    };

                    _emailQueueBAL.Enqueue(emailRequest);

                }

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserCrediential(UpdateUserRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _authBAL.UpdateUserCrediential(request, userId);
                var responseMessage = result;
                var updatedUser = await _authRepository.GetUserByUserId(request.UserId);

                if (!string.IsNullOrWhiteSpace(updatedUser?.Email))
                {
                    var updatedByUser = await _authRepository.GetUserByUserId(userId);

                    var emailRequest = new SendEmailRequest
                    {
                        ToEmails = new List<string> { request.Email },
                        TemplateType = "PasswordUpdate",
                        CompanyName = "StarPropertiesAndRealty",
                        LoginUrl = string.Empty,
                        UserName = updatedUser.Name,
                        Username = updatedUser.Username,
                        Email = request.Email,
                        Role = updatedUser.Role,
                        Password = request.Password,
                        UpdatedByName = updatedByUser?.Name ?? updatedByUser?.Username ?? "Admin",
                        UpdatedByEmail = updatedByUser?.Email ?? string.Empty,
                        UpdatedDateTime = DateTime.UtcNow
                    };
                    _emailQueueBAL.Enqueue(emailRequest);

                }

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserCrediential(DeleteUserCredientialsRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _authBAL.DeleteUserCrediential(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCrediential()
        {
            try
            {
                var result = await _authBAL.GetUserCrediential();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TrackVisitor()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            await _authBAL.TrackVisitor(ip, userAgent);

            return Ok();
        }

        // ✅ Dashboard Count
        [HttpGet]
        public async Task<IActionResult> GetVisitorDashboard()
        {
            var result = await _authBAL.GetVisitorDashboard();
            return Ok(result);
        }
    }
}
