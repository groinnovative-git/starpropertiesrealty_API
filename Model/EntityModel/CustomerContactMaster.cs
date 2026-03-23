using System.ComponentModel.DataAnnotations;

namespace Star_Properties.Model.EntityModel
{
    public class CustomerContactMaster
    {
        [Key]
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerInterest { get; set; }
        public string Message { get; set; }
        public Guid? PropertyId { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
