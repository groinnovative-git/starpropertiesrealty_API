using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace Star_Properties.Model.EntityModel
{
    public class PropertiesDetailsMaster
    {
        [Key]
        public Guid PropertiesDetailsId { get; set; }

        // BASIC
        public string? PropertyTitle { get; set; }
        public decimal? Price { get; set; }
        public string? PropertyType { get; set; }
        public string? PropertySubType { get; set; }
        public bool IsLoanProviding { get; set; }
        public string? PropertyStatus { get; set; } = "Active";
        // SPEC
        public decimal? PropertySqFt { get; set; }
        public decimal? PlotAreaSqYd { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? NumberOfFloors { get; set; }
        public int? FloorNumber { get; set; }
        public decimal? MonthlyMaintenance { get; set; }
        public int? Washrooms { get; set; }
        public string? CommercialType { get; set; }
        public bool? IsGovApproved { get; set; }

        // ATTRIBUTES
        public string? FurnishingStatus { get; set; }
        public string? FacingDirection { get; set; }
        public int? AgeOfProperty { get; set; }

        // AMENITIES
        public bool? HasSwimmingPool { get; set; }
        public bool? HasGym { get; set; }
        public bool? HasSecurity { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasClubHouse { get; set; }
        public bool? HasPowerBackup { get; set; }
        public bool? HasLift { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasKidsPlayArea { get; set; }
        public bool? HasCCTV { get; set; }
        public bool? HasIntercom { get; set; }
        public bool? HasFireSafety { get; set; }
        public bool? HasWaterSupply24x7 { get; set; }

        // MEDIA
        //public List<string>? ImageUrls { get; set; }
        public string ImageUrls { get; set; }
        public string? VideoUrl1 { get; set; }
        public string? VideoUrl2 { get; set; }

        // DETAILS
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? LocationIframe { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
