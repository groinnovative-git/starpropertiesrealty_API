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
        public string? PlotDimensions { get; set; }
        public string? TotalLandArea { get; set; }
        public decimal? PricePerAcre { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? NumberOfFloors { get; set; }
        public int? FloorNumber { get; set; }
        public string? FloorDetails { get; set; }
        public decimal? MonthlyMaintenance { get; set; }
        public int? Washrooms { get; set; }
        public string? CommercialType { get; set; }
        public string? LandType { get; set; }
        public string? GovApprovedCertificate { get; set; }

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
        public bool? HasVisitorParking { get; set; }
        public bool? HasGatedCommunity { get; set; }
        public bool? HasPartyHall { get; set; }
        public bool? HasPark { get; set; }
        public bool? HasWalkingTrack { get; set; }
        public bool? HasRainwaterHarvesting { get; set; }
        public bool? HasWasteManagement { get; set; }
        public bool? HasSeniorCitizenArea { get; set; }
        public bool? HasTerrace { get; set; }
        public bool? HasBalcony { get; set; }
        public bool? HasServantRoom { get; set; }
        public bool? HasSolarPower { get; set; }
        public bool? HasEVChargingPoint { get; set; }
        public bool? HasBlackTopRoad { get; set; }
        public bool? HasCornerPlot { get; set; }
        public bool? HasStreetLights { get; set; }
        public bool? HasDrainageConnection { get; set; }
        public bool? HasWaterConnection { get; set; }
        public bool? HasElectricityConnection { get; set; }
        public bool? HasUndergroundSewage { get; set; }
        public bool? HasAvenueTrees { get; set; }
        public bool? HasCompoundWall { get; set; }
        public bool? HasFencing { get; set; }
        public bool? IsReadyForConstruction { get; set; }
        public bool? HasRoadAccess { get; set; }
        public bool? HasWaterSource { get; set; }
        public bool? HasBorewell { get; set; }
        public bool? HasDripIrrigation { get; set; }
        public bool? HasSprinklerSystem { get; set; }
        public bool? HasFarmHouse { get; set; }
        public bool? HasStorageShed { get; set; }
        public bool? HasCattleShed { get; set; }
        public bool? HasWatchmanRoom { get; set; }
        public bool? HasSolarPump { get; set; }
        public bool? HasTreePlantation { get; set; }
        public bool? IsOrganicFarmingReady { get; set; }
        public bool? HasRiverAccess { get; set; }
        public bool? HasLakeView { get; set; }
        public bool? HasHillView { get; set; }
        public bool? HasPrivateEntrance { get; set; }
        public bool? HasMunicipalityWaterSupply { get; set; }
        public bool? HasStoreRoom { get; set; }
        public bool? HasModularKitchen { get; set; }
        public bool? HasWiFi { get; set; }
        public bool? HasCentralizedAC { get; set; }
        public bool? HasReceptionArea { get; set; }
        public bool? HasConferenceRoom { get; set; }
        public bool? HasPantry { get; set; }
        public bool? HasRestrooms { get; set; }
        public bool? HasServiceLift { get; set; }
        public bool? HasLoadingBay { get; set; }
        public bool? HasWheelchairAccess { get; set; }
        public bool? HasMaintenanceStaff { get; set; }
        public bool? HasGeneratorBackup { get; set; }
        public decimal? PropertyLoanPercentage { get; set; }
        public string? HospitalDistance { get; set; }
        public string? CollegeDistance { get; set; }
        public string? SchoolDistance { get; set; }
        public string? BusStandDistance { get; set; }
        public bool? IsRental { get; set; }
        // PG / ROOM AMENITIES
        public bool? HasFoodIncluded { get; set; }
        public bool? HasWashingMachine { get; set; }
        public bool? HasHousekeeping { get; set; }
        public bool? HasBed { get; set; }
        public bool? HasCupboard { get; set; }
        public bool? HasTable { get; set; }
        public bool? HasChair { get; set; }
        public bool? HasAC { get; set; }
        public bool? HasTV { get; set; }
        public bool? HasGeyser { get; set; }
        public bool? HasSecurityGuard { get; set; }
        public bool? HasSharedKitchen { get; set; }
        public bool? IsCookingAllowed { get; set; }
        public decimal? MonthlyRent { get; set; }
        public decimal? DepositAmount { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public string? SharingType { get; set; }
        public string? GenderAllowed { get; set; }
        public bool? HasAttachedBathroom { get; set; }
        public bool? IsFurnished { get; set; }
        public bool? IsSale { get; set; }

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
