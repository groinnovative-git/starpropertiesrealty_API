using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class VisitorTracking
    {
        [Key]
        public Guid VisitorId { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTime VisitedOn { get; set; }
    }
}
