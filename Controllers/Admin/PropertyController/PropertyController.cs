using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Star_Properties.BAL.Interface.IPropertyBAL;
using Star_Properties.Model.RequestModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star_Properties.Controllers.Admin.PropertyController
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyBAL _bal;

        public PropertyController(IPropertyBAL bal)
        {
            _bal = bal;
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

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProperty([FromForm]PropertyRequest req)
        {
            var id = await _bal.AddProperty(req, GetUserIdFromToken());
            return Ok(new { PropertyId = id });
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProperty([FromForm]PropertyRequest req)
        {
            await _bal.UpdateProperty(req, GetUserIdFromToken());
            return Ok("Updated successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            var result = await _bal.GetAllProperties();
            return Ok(result);   
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertiesById(Guid propertyId)
        {
            var result = await _bal.GetPropertiesById(propertyId);  
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProperty(Guid propertyId)
        {
            await _bal.DeleteProperty(propertyId);
            return Ok("Deleted successfully");
        }
    }
}
