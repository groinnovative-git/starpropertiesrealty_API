namespace Star_Properties.Model.EntityModel
{
    public class PropertyAudit
    {
        public Guid PropertyAuditId { get; set; }
        public Guid PropertyId { get; set; }

        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public string ActionType { get; set; }

        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
