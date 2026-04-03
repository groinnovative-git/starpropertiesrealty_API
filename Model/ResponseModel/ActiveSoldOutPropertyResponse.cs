namespace Star_Properties.Model.ResponseModel
{
    public class ActiveSoldOutPropertyResponse
    {
        public Guid PropertiesDetailsId { get; set; }

        public string PropertyTitle { get; set; }

        public decimal Price { get; set; }

        public string PropertyType { get; set; }

        public string PropertyStatus { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
