using Microsoft.AspNetCore.Mvc;
using Star_Properties.BAL.Interface.ICustomerContactBAL;
using Star_Properties.Model.RequestModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star_Properties.Controllers.Admin.CustomerController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactBAL _bal;
        public CustomerContactController(ICustomerContactBAL bal)
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

        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] CustomerContactRequest request)
        {
            var response = await _bal.SubmitContact(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var response = await _bal.GetAllContacts();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromBody] CustomerContactRequest request)
        {
            var userId = GetUserIdFromToken(); 
            var response = await _bal.UpdateContact(request, userId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetContactAuditDetails(Guid contactId)
        {
            var response = await _bal.GetContactAuditDetails(contactId);
            return Ok(response);
        }
    }
}
