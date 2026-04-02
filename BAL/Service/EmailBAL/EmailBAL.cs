using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Star_Properties.BAL.Interface.IEmailBAL;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Repository.Interface.IEmailRepository;
using static System.Net.Mime.MediaTypeNames;

namespace Star_Properties.BAL.Service.EmailBAL
{
    public class EmailBAL : IEmailBAL
    {
        private readonly IEmailRepository _repo;
        private readonly SmtpSettings _smtp;
        private readonly IConfiguration _appsettings;
        private readonly ApplicationDbContext _context;

        public EmailBAL(
            IEmailRepository repo,
            IOptions<SmtpSettings> smtp,
            IConfiguration appsettings,
            ApplicationDbContext context)
        {
            _repo = repo;
            _smtp = smtp.Value;
            _appsettings = appsettings;
            _context = context;
        }

        public async Task<bool> SendEmail(SendEmailRequest request, HttpContext httpContext)
        {
            var log = new EmailLog
            {
                EmailId = Guid.NewGuid(),
                FromEmail = _smtp.Username,
                ToEmail = "",
                Subject = "",
                Status = "Pending",
                CreatedOn = DateTime.UtcNow
            };

            try
            {
                if (request.ToEmails == null || !request.ToEmails.Any())
                    throw new Exception("Employee email is required");

                // GET ADMIN EMAIL FROM DB
                var adminEmail = _context.UserMaster
                    .Where(x => x.Role == "Admin" && x.IsActive)
                    .Select(x => x.Email)
                    .FirstOrDefault();

                // MERGE EMPLOYEE + ADMIN
                var toEmails = request.ToEmails.ToList();

                if (!string.IsNullOrWhiteSpace(adminEmail) &&
                    !toEmails.Contains(adminEmail, StringComparer.OrdinalIgnoreCase))
                {
                    toEmails.Add(adminEmail);
                }

                log.ToEmail = string.Join(",", toEmails);

                using var smtp = new SmtpClient();
                smtp.Timeout = 60000;

                await smtp.ConnectAsync(
                    _smtp.Host,
                    _smtp.Port ?? 587,
                    _smtp.UseSsl == true
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto
                );

                await smtp.AuthenticateAsync(_smtp.Username, _smtp.Password);

                // LOOP ALL EMAILS (ADMIN + EMPLOYEE)
                foreach (var to in toEmails)
                {
                    if (string.IsNullOrWhiteSpace(to))
                        continue;

                    // GET ROLE
                    var user = _context.UserMaster
                        .Where(x => x.Email == to && x.IsActive)
                        .Select(x => new { x.Role })
                        .FirstOrDefault();

                    if (user == null)
                        continue;

                    string subject;
                    string templatePath;

                    // ROLE BASED TEMPLATE
                    if (user.Role == "Admin")
                    {
                        subject = "New Account Created";
                        templatePath = "Templates/UserCreatedTemplate.html";
                    }
                    else
                    {
                        subject = "Account Updated";
                        templatePath = "Templates/UserUpdatedTemplate.html";
                    }

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

                    if (!File.Exists(fullPath))
                        continue;

                    var html = await File.ReadAllTextAsync(fullPath);

                    // LOGO
                    var logoUrl = "https://land.studiomart.in/images/logo_light.png";
                    var logoHtml = $@"<img src=""{logoUrl}"" width=""140"" style=""display:block; margin:0 auto;"" />";

                    // REPLACE VALUES
                    html = html
                        .Replace("{{companyName}}", request.CompanyName ?? "Star Properties")
                        .Replace("{{employeeName}}", request.EmployeeName ?? "")
                        .Replace("{{role}}", request.Role ?? "")
                        .Replace("{{username}}", request.Username ?? "")
                        .Replace("{{email}}", to)
                        .Replace("{{password}}", request.Password ?? "")
                        .Replace("{{createdByName}}", request.CreatedByName ?? "")
                        .Replace("{{createdByUsername}}", request.CreatedByUsername ?? "")
                        .Replace("{{createdByEmail}}", request.CreatedByEmail ?? "")
                        .Replace("{{createdDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"))
                        .Replace("{{logo}}", logoHtml);

                    var email = new MimeMessage();

                    email.From.Add(new MailboxAddress("Star Properties", _smtp.Username));
                    email.To.Add(MailboxAddress.Parse(to));
                    email.Subject = subject;

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = html,
                        TextBody = "Email Notification"
                    };

                    email.Body = bodyBuilder.ToMessageBody();

                    await smtp.SendAsync(email);

                    await Task.Delay(800); 
                }

                await smtp.DisconnectAsync(true);

                log.Status = "Sent";
                log.Body = "Emails sent successfully";

                await _repo.SaveEmailLog(log);

                return true;
            }
            catch (Exception ex)
            {
                log.Status = "Failed";
                log.Body = ex.Message;

                await _repo.SaveEmailLog(log);

                return false;
            }
        }
    }
}






//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using MimeKit.Utils;
//using Star_Properties.BAL.Interface.IEmailBAL;
//using Star_Properties.Model.EntityModel;
//using Star_Properties.Model.RequestModel;
//using Star_Properties.Repository.Interface.IEmailRepository;


//namespace Star_Properties.BAL.Service.EmailBAL
//{
//    public class EmailBAL : IEmailBAL
//    {
//        private readonly IEmailRepository _repo;
//        private readonly SmtpSettings _smtp;
//        private readonly IConfiguration _appsettings;

//        public EmailBAL(IEmailRepository repo, IOptions<SmtpSettings> smtp, IConfiguration appsettings)
//        {
//            _repo = repo;
//            _smtp = smtp.Value;
//            _appsettings = appsettings;
//        }

//        public async Task<bool> SendEmail(SendEmailRequest request)
//        {
//            var log = new EmailLog
//            {
//                EmailId = Guid.NewGuid(),
//                FromEmail = _smtp.Username,
//                ToEmail = request.ToEmail,
//                Subject = "",
//                Status = "Pending",
//                CreatedOn = DateTime.UtcNow,
//            };

//            try
//            {
//                // TEMPLATE CONFIG MODEL
//                var templates = new Dictionary<string, (string subject, string path, bool useSupportEmail)>
//                {
//                    { "UserCreatedTemplate", ("New Account Created", "Templates/UserCreatedTemplate.html", true) },
//                    { "UserUpdatedTemplate", ("Account Has Been Created", "Templates/UserUpdatedTemplate.html", true) }
//                };

//                if (!templates.ContainsKey(request.TemplateName))
//                    throw new Exception("Invalid Template");

//                var template = templates[request.TemplateName];

//                log.Subject = template.subject;

//                // 🔹 Set From Email
//                log.FromEmail = template.useSupportEmail
//                    ? (_appsettings["App:SupportEmail"] ?? _smtp.Username)
//                    : _smtp.Username;

//                // 🔹 Read template
//                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), template.path);

//                if (!File.Exists(templatePath))
//                    throw new Exception("Template file not found");

//                var html = await File.ReadAllTextAsync(templatePath);

//                // Replace placeholders
//                html = html
//                    .Replace("{{companyName}}", request.CompanyName ?? "Star Properties")
//                    .Replace("{{employeeName}}", request.EmployeeName ?? "")
//                    .Replace("{{role}}", request.Role ?? "")
//                    .Replace("{{username}}", request.Username ?? "")
//                    .Replace("{{email}}", request.ToEmail ?? "")
//                    .Replace("{{password}}", request.Password ?? "")
//                    .Replace("{{createdByName}}", request.CreatedByName ?? "")
//                    .Replace("{{createdByUsername}}", request.CreatedByUsername ?? "")
//                    .Replace("{{createdByEmail}}", request.CreatedByEmail ?? "")
//                    .Replace("{{createdDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"));

//                // Build Email
//                var email = new MimeMessage();
//                email.From.Add(new MailboxAddress("Star Properties", request.FromEmail));
//                email.To.Add(MailboxAddress.Parse(request.ToEmail));
//                email.Subject = template.subject;

//                var body = new BodyBuilder
//                {
//                    HtmlBody = html,
//                    TextBody = "Email Notification"
//                };

//                // EMBED LOGO (CID)
//                //var logoPath = Path.Combine(
//                //    Directory.GetCurrentDirectory(),
//                //    "wwwroot",
//                //    "images",
//                //    "StarPropertiesLogo.png"
//                //);

//                //if (File.Exists(logoPath))
//                //{
//                //    var logo = body.LinkedResources.Add(logoPath);
//                //    logo.ContentId = "logo_cid";
//                //    logo.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
//                //}

//                // EMBED LOGO (CID)
//                var logoPath = Path.Combine(
//                    Directory.GetCurrentDirectory(),
//                    "wwwroot",
//                    "images",
//                    "StarPropertiesLogo.jpg"
//                );

//                if (File.Exists(logoPath))
//                {
//                    var logo = body.LinkedResources.Add(logoPath);

//                    // Generate dynamic CID
//                    logo.ContentId = MimeUtils.GenerateMessageId();

//                    logo.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

//                    // Replace HTML AFTER generating CID
//                    html = html.Replace("cid:logo_cid", $"cid:{logo.ContentId}");

//                    // Update body again
//                    body.HtmlBody = html;
//                }

//                email.Body = body.ToMessageBody();

//                // Send Email
//                using var smtp = new SmtpClient();
//                smtp.Timeout = 60000;

//                await smtp.ConnectAsync(
//                    _smtp.Host,
//                    _smtp.Port ?? 587,
//                    _smtp.UseSsl == true
//                        ? SecureSocketOptions.StartTls
//                        : SecureSocketOptions.Auto
//                );

//                await smtp.AuthenticateAsync(_smtp.Username, _smtp.Password);
//                await smtp.SendAsync(email);
//                await smtp.DisconnectAsync(true);

//                // Log Success
//                log.Status = "Sent";
//                log.Body = html;

//                await _repo.SaveEmailLog(log);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                log.Status = "Failed";
//                log.Body = ex.Message;

//                await _repo.SaveEmailLog(log);

//                return false;
//            }
//        }
//    }
//}
