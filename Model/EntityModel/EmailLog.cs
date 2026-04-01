using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class EmailLog
    {
        [Key]
        public Guid EmailId { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
