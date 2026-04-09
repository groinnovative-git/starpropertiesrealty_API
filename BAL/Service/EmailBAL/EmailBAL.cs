using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Star_Properties.BAL.Interface.IEmailBAL;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Repository.Interface.IEmailRepository;

namespace Star_Properties.BAL.Service.EmailBAL
{
    public class EmailBAL : IEmailBAL
    {
        private readonly IEmailRepository _repo;
        private readonly SmtpSettings _smtp;
        private readonly IConfiguration _appsettings;
        private readonly ApplicationDbContext _context;

        public EmailBAL(IEmailRepository repo,IOptions<SmtpSettings> smtp,IConfiguration appsettings,ApplicationDbContext context)
        {
            _repo = repo;
            _smtp = smtp.Value;
            _appsettings = appsettings;
            _context = context;
        }

        public async Task<bool> SendEmail(SendEmailRequest request, HttpContext httpContext)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(request.TemplateType))
                    throw new Exception("TemplateType is required");

                var templatePairs = new Dictionary<string, (string adminTemplate, string employeeTemplate, string adminSubject, string employeeSubject)>
                {
                    {
                        "CreateCredential",
                        (
                            "Templates/CreatedCredientialsAdmin.html",
                            "Templates/CreateCredientialsEmployee.html",
                            "New Account Created",
                            "Your Account Created"
                        )
                    },
                    {
                        "PasswordUpdate",
                        (
                            "Templates/PasswordUpdateForAdmin.html",
                            "Templates/PasswordUpdateForEmployee.html",
                            "Password Updated - Admin Notification",
                            "Your Password Updated Successfully"
                        )
                    },
                    {
                        "CustomerContact",
                        (
                            "Templates/CustomerContactForAdmin.html",
                            "Templates/CustomerContactForEmployee.html",
                            "New Contact Inquiry",
                            "We Received Your Inquiry"
                        )
                    },
                    {
                        "PropertyContact",
                        (
                            "Templates/PropertyContactForAdmin.html",
                            "Templates/PropertyContactForEmployee.html",
                            "New Property Inquiry",
                            "Your Visit Request Received"
                        )
                    }
                };

                if (!templatePairs.ContainsKey(request.TemplateType))
                    throw new Exception("Invalid TemplateType");

                var pair = templatePairs[request.TemplateType];

                // GET ADMIN EMAIL
                var adminEmail = _context.UserMaster
                    .Where(x => x.Role == "Admin" && x.IsActive)
                    .Select(x => x.Email)
                    .FirstOrDefault();

                var employeeEmails = request.ToEmails ?? new List<string>();

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

                if (!string.IsNullOrWhiteSpace(adminEmail))
                {
                    await SendMail(
                        smtp,
                        adminEmail,
                        pair.adminSubject,
                        pair.adminTemplate,
                        request
                    );
                }

                foreach (var emp in employeeEmails)
                {
                    if (string.IsNullOrWhiteSpace(emp)) continue;

                    await SendMail(
                        smtp,
                        emp,
                        pair.employeeSubject,
                        pair.employeeTemplate,
                        request
                    );
                }

                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task SendMail(SmtpClient smtp,string to,string subject,string templatePath,SendEmailRequest request)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

            if (!File.Exists(fullPath))
                return;

            var html = await File.ReadAllTextAsync(fullPath);

            html = html
                .Replace("{{companyName}}", "StarPropertiesAndRealty")

                .Replace("{{userName}}", request.UserName ?? "")
                .Replace("{{employeeName}}", request.UserName ?? "")
                .Replace("{{username}}", request.Username ?? "")
                .Replace("{{password}}", request.Password ?? "")

                .Replace("{{email}}", request.Email ?? to ?? "")
                .Replace("{{role}}", request.Role ?? "")
                .Replace("{{updatedDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"))

                // Admin
                .Replace("{{createdByName}}", request.CreatedByName ?? "")
                .Replace("{{createdByEmail}}", request.CreatedByEmail ?? "")
                .Replace("{{updatedByName}}", request.UpdatedByName ?? "")
                .Replace("{{updatedByEmail}}", request.UpdatedByEmail ?? "")

                // Contact / Visit
                .Replace("{{fullName}}", request.FullName ?? "")
                .Replace("{{phone}}", request.Phone ?? "")
                .Replace("{{visitDate}}", request.VisitDate ?? "")
                .Replace("{{propertyName}}", request.PropertyName ?? "")
                .Replace("{{message}}", request.Message ?? "")
                .Replace("{{interest}}", request.Interest ?? "")
                .Replace("{{createdDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"))

                // Links
                .Replace("{{loginUrl}}", request.LoginUrl ?? "");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("StarPropertiesAndRealty", _smtp.Username));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = html,
                TextBody = "Email Notification"
            };

            email.Body = bodyBuilder.ToMessageBody();

            await smtp.SendAsync(email);

            await Task.Delay(300); 
        }
    }
}
