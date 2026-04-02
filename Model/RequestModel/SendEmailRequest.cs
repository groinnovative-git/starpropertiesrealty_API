namespace Star_Properties.Model.RequestModel
{

    public class SendEmailRequest
    {
        public List<string> ToEmails { get; set; } = new();
        public string TemplateType { get; set; }
        public string? FromEmail { get; set; }
        public string? CompanyName { get; set; }
        public string? LoginUrl { get; set; }

        public string? UserName { get; set; } 
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }

        public string? CreatedByName { get; set; }
        public string? CreatedByEmail { get; set; }

        public string? UpdatedByName { get; set; }
        public string? UpdatedByEmail { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Message { get; set; }
        public string? Interest { get; set; }

        public string? PropertyName { get; set; }
        public string? VisitDate { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }

    //public class SendEmailRequest
    //{
    //    //public string ToEmail { get; set; }
    //    public List<string> ToEmails { get; set; }
    //    public string TemplateName { get; set; }

    //    public string? FromEmail { get; set; }

    //    // Admin Template
    //    public string? CreatedByName { get; set; }
    //    public string? CreatedByUsername { get; set; }
    //    public string? CreatedByEmail { get; set; }

    //    // Employee Template
    //    public string? EmployeeName { get; set; }
    //    public string? Role { get; set; }
    //    public string? Username { get; set; }
    //    public string? Password { get; set; }
    //    public string? CompanyName { get; set; }
    //}
}
