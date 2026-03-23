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

        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] CustomerContactRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _bal.SubmitContact(request);
            return Ok(response);
        }
    }
}
