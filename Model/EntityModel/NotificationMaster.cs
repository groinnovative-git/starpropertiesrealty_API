using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class NotificationMaster
    {
        [Key]
        public Guid NotificationId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
