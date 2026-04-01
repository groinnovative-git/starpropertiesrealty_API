using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Star_Properties.BAL.Interface.IEmailBAL;
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

        public EmailBAL(
            IEmailRepository repo,
            IOptions<SmtpSettings> smtp,
            IConfiguration appsettings)
        {
            _repo = repo;
            _smtp = smtp.Value;
            _appsettings = appsettings;
        }

        public async Task<bool> SendEmail(SendEmailRequest request, HttpContext httpContext)
        {
            var log = new EmailLog
            {
                EmailId = Guid.NewGuid(),
                FromEmail = _smtp.Username,
                ToEmail = request.ToEmail,
                Subject = "",
                Status = "Pending",
                CreatedOn = DateTime.UtcNow
            };

            try
            {
                // 🔹 TEMPLATE CONFIG
                var templates = new Dictionary<string, (string subject, string path, bool useSupportEmail)>
                {
                    { "UserCreatedTemplate", ("New Account Created", "Templates/UserCreatedTemplate.html", true) },
                    { "UserUpdatedTemplate", ("Account Has Been Updated", "Templates/UserUpdatedTemplate.html", true) }
                };

                if (!templates.TryGetValue(request.TemplateName, out var template))
                    throw new Exception("Invalid Template");

                log.Subject = template.subject;

                // 🔹 FROM EMAIL
                log.FromEmail = template.useSupportEmail
                    ? (_appsettings["App:SupportEmail"] ?? _smtp.Username)
                    : _smtp.Username;

                // 🔹 READ TEMPLATE
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), template.path);

                if (!File.Exists(templatePath))
                    throw new Exception("Template file not found");

                var html = await File.ReadAllTextAsync(templatePath);

                // 🔥 DYNAMIC BASE URL
                var req = httpContext.Request;
                string baseUrl = $"{req.Scheme}://{req.Host}";

                //string appLogoUrl = $"{baseUrl}/images/StarPropertiesLogo.png";
                var appLogoUrl = "https://land.studiomart.in/images/logo_light.png";

                string appLogoCid = "starproperties-logo";

                // 🔥 CHECK GMAIL
                bool isGmailRecipient =
                    request.ToEmail?.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) == true ||
                    request.ToEmail?.EndsWith("@googlemail.com", StringComparison.OrdinalIgnoreCase) == true;

                // 🔥 LOGO HTML
                var appLogoHtml = isGmailRecipient
                    ? $@"<img src=""{appLogoUrl}"" alt=""{request.CompanyName ?? "StarProperties"}"" width=""140"" style=""display:block; margin:0 auto;"" />"
                    : $@"<!--[if mso]>
                            <img src=""cid:{appLogoCid}"" alt=""{request.CompanyName ?? "StarProperties"}"" width=""140"" style=""display:block; margin:0 auto;"" />
                        <![endif]-->
                        <!--[if !mso]><!-- -->
                            <img src=""{appLogoUrl}"" alt=""{request.CompanyName ?? "StarProperties"}"" width=""140"" style=""display:block; margin:0 auto;"" />
                        <!--<![endif]-->";


            // 🔹 REPLACE PLACEHOLDERS
            html = html
                    .Replace("{{companyName}}", request.CompanyName ?? "Star Properties")
                    .Replace("{{employeeName}}", request.EmployeeName ?? "")
                    .Replace("{{role}}", request.Role ?? "")
                    .Replace("{{username}}", request.Username ?? "")
                    .Replace("{{email}}", request.ToEmail ?? "")
                    .Replace("{{password}}", request.Password ?? "")
                    .Replace("{{createdByName}}", request.CreatedByName ?? "")
                    .Replace("{{createdByUsername}}", request.CreatedByUsername ?? "")
                    .Replace("{{createdByEmail}}", request.CreatedByEmail ?? "")
                    .Replace("{{createdDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"))
                    .Replace("{{logo}}", appLogoHtml);


                // 🔹 BUILD EMAIL
                var email = new MimeMessage();

                var fromEmail = string.IsNullOrWhiteSpace(log.FromEmail)
                    ? _smtp.Username
                    : log.FromEmail;

                email.From.Add(new MailboxAddress("Star Properties", fromEmail));
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = template.subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = html,
                    TextBody = "Email Notification"
                };

                // 🔥 INLINE ONLY FOR NON-GMAIL
                if (!isGmailRecipient)
                {
                    try
                    {
                        using var http = new HttpClient();
                        using var logoStream = await http.GetStreamAsync(appLogoUrl);

                        var logo = bodyBuilder.LinkedResources.Add("StarPropertiesLogo.png", logoStream);

                        logo.ContentId = appLogoCid;
                        logo.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
                        logo.ContentLocation = new Uri(appLogoUrl);
                    }
                    catch
                    {
                        // fallback → use URL only
                        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody
                            .Replace($@"src=""cid:{appLogoCid}""", $@"src=""{appLogoUrl}""");
                    }
                }

                email.Body = bodyBuilder.ToMessageBody();

                // 🔹 SEND EMAIL
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
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                // 🔹 LOG SUCCESS
                log.Status = "Sent";
                log.Body = html;

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
