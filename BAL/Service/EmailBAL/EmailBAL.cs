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
                if (string.IsNullOrWhiteSpace(request.TemplateType))
                    throw new Exception("TemplateType is required");

                // =============================
                // TEMPLATE PAIRS (ADMIN + EMPLOYEE)
                // =============================
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

                // =============================
                // GET ADMIN EMAIL
                // =============================
                var adminEmail = _context.UserMaster
                    .Where(x => x.Role == "Admin" && x.IsActive)
                    .Select(x => x.Email)
                    .FirstOrDefault();

                var employeeEmails = request.ToEmails ?? new List<string>();

                log.ToEmail = string.Join(",", employeeEmails) + "," + adminEmail;

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

        private async Task SendMail(
            SmtpClient smtp,
            string to,
            string subject,
            string templatePath,
            SendEmailRequest request)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

            if (!File.Exists(fullPath))
                return;

            var html = await File.ReadAllTextAsync(fullPath);

            html = html
                .Replace("{{companyName}}", "StarPropertiesAndRealty")

                // ✅ FIX HERE
                .Replace("{{userName}}", request.UserName ?? "")
                .Replace("{{employeeName}}", request.UserName ?? "")
                .Replace("{{username}}", request.UserName ?? "")
                .Replace("{{password}}", request.UserName ?? "")

                .Replace("{{email}}", to ?? "")
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

            await Task.Delay(300); // small delay for stability
        }
    

    //public async Task<bool> SendEmail(SendEmailRequest request, HttpContext httpContext)
    //{
    //    var log = new EmailLog
    //    {
    //        EmailId = Guid.NewGuid(),
    //        FromEmail = _smtp.Username,
    //        ToEmail = "",
    //        Subject = "",
    //        Status = "Pending",
    //        CreatedOn = DateTime.UtcNow
    //    };

    //    try
    //    {
    //        if (request.ToEmails == null || !request.ToEmails.Any())
    //            throw new Exception("Employee email is required");

    //        // GET ADMIN EMAIL FROM DB
    //        var adminEmail = _context.UserMaster
    //            .Where(x => x.Role == "Admin" && x.IsActive)
    //            .Select(x => x.Email)
    //            .FirstOrDefault();

    //        // MERGE EMPLOYEE + ADMIN
    //        var toEmails = request.ToEmails.ToList();

    //        if (!string.IsNullOrWhiteSpace(adminEmail) &&
    //            !toEmails.Contains(adminEmail, StringComparer.OrdinalIgnoreCase))
    //        {
    //            toEmails.Add(adminEmail);
    //        }

    //        log.ToEmail = string.Join(",", toEmails);

    //        using var smtp = new SmtpClient();
    //        smtp.Timeout = 60000;

    //        await smtp.ConnectAsync(
    //            _smtp.Host,
    //            _smtp.Port ?? 587,
    //            _smtp.UseSsl == true
    //                ? SecureSocketOptions.StartTls
    //                : SecureSocketOptions.Auto
    //        );

    //        await smtp.AuthenticateAsync(_smtp.Username, _smtp.Password);

    //        // LOOP ALL EMAILS (ADMIN + EMPLOYEE)
    //        foreach (var to in toEmails)
    //        {
    //            if (string.IsNullOrWhiteSpace(to))
    //                continue;

    //            // GET ROLE
    //            var user = _context.UserMaster
    //                .Where(x => x.Email == to && x.IsActive)
    //                .Select(x => new { x.Role })
    //                .FirstOrDefault();

    //            if (user == null)
    //                continue;

    //            string subject;
    //            string templatePath;

    //            // ROLE BASED TEMPLATE
    //            if (user.Role == "Admin")
    //            {
    //                subject = "New Account Created";
    //                templatePath = "Templates/CreatedCredientialsAdmin.html";
    //            }
    //            else
    //            {
    //                subject = "Account Updated";
    //                templatePath = "Templates/CreateCredientialsEmployee.html";
    //            }

    //            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

    //            if (!File.Exists(fullPath))
    //                continue;

    //            var html = await File.ReadAllTextAsync(fullPath);

    //            // LOGO
    //            var logoUrl = "https://land.studiomart.in/images/logo_light.png";
    //            var logoHtml = $@"<img src=""{logoUrl}"" width=""140"" style=""display:block; margin:0 auto;"" />";

    //            // REPLACE VALUES
    //            html = html
    //                .Replace("{{companyName}}", request.CompanyName ?? "Star Properties")
    //                .Replace("{{employeeName}}", request.EmployeeName ?? "")
    //                .Replace("{{role}}", request.Role ?? "")
    //                .Replace("{{username}}", request.Username ?? "")
    //                .Replace("{{email}}", to)
    //                .Replace("{{password}}", request.Password ?? "")
    //                .Replace("{{createdByName}}", request.CreatedByName ?? "")
    //                .Replace("{{createdByUsername}}", request.CreatedByUsername ?? "")
    //                .Replace("{{createdByEmail}}", request.CreatedByEmail ?? "")
    //                .Replace("{{createdDateTime}}", DateTime.Now.ToString("dd MMM yyyy hh:mm tt"))
    //                .Replace("{{logo}}", logoHtml);

    //            var email = new MimeMessage();

    //            email.From.Add(new MailboxAddress("Star Properties", _smtp.Username));
    //            email.To.Add(MailboxAddress.Parse(to));
    //            email.Subject = subject;

    //            var bodyBuilder = new BodyBuilder
    //            {
    //                HtmlBody = html,
    //                TextBody = "Email Notification"
    //            };

    //            email.Body = bodyBuilder.ToMessageBody();

    //            await smtp.SendAsync(email);

    //            await Task.Delay(800); 
    //        }

    //        await smtp.DisconnectAsync(true);

    //        log.Status = "Sent";
    //        log.Body = "Emails sent successfully";

    //        await _repo.SaveEmailLog(log);

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Status = "Failed";
    //        log.Body = ex.Message;

    //        await _repo.SaveEmailLog(log);

    //        return false;
    //    }
    //}
}
}
