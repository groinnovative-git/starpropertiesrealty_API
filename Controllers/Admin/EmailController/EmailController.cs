using Microsoft.AspNetCore.Mvc;
using Star_Properties.BAL.Interface.IEmailBAL;
using Star_Properties.Model.RequestModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star_Properties.Controllers.Admin.EmailController
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailBAL _bal;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailBAL bal, ILogger<EmailController> logger)
        {
            _bal = bal;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.ToEmail))
                return BadRequest("Email is required");

            var result = await _bal.SendEmail(req, HttpContext);

            return result ? Ok("Email sent successfully") : BadRequest("Email failed");
        }
    }
}
