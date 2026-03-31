namespace Star_Properties.Model.ResponseModel
{
    public class CustomerContactResponse
    {
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerInterest { get; set; }
        public string Message { get; set; }
        public Guid? PropertyId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string LeadStatus { get; set; } 
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
