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
                return Unauthorized("Invalid email or password");
            }
        }
    }
}
