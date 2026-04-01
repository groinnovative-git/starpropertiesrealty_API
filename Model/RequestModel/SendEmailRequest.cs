namespace Star_Properties.Model.RequestModel
{
    public class SendEmailRequest
    {
        public string ToEmail { get; set; }
        public string TemplateName { get; set; }

        public string? FromEmail { get; set; }

        // Admin Template
        public string? CreatedByName { get; set; }
        public string? CreatedByUsername { get; set; }
        public string? CreatedByEmail { get; set; }

        // Employee Template
        public string? EmployeeName { get; set; }
        public string? Role { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? CompanyName { get; set; }
    }
}
