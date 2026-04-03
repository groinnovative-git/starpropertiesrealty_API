using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class NotificationUserMapping
    {
        [Key]
        public Guid NotificationUserMappingId { get; set; }

        public Guid NotificationId { get; set; }

        public Guid UserId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadOn { get; set; }
    }
}
