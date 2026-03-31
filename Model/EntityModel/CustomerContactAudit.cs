using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class CustomerContactAudit
    {
        [Key]
        public Guid AuditId { get; set; }
        public Guid ContactId { get; set; }

        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
    }
}
