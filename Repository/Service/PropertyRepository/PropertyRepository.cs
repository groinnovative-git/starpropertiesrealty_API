using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Webp;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.IPropertyRepository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Star_Properties.Repository.Service.PropertyRepository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // ADD PROPERTY
        // ==========================
        public async Task<(Guid, string)> AddProperty(PropertyRequest req, Guid userId)
        {

            var uploadedImages = await SaveImagesAsync(req.Images);
            var frontendImages = GetFrontendImages(req);

            var finalImages = uploadedImages.Any() ? uploadedImages : frontendImages;

            var property = new PropertiesDetailsMaster
            {
                PropertiesDetailsId = Guid.NewGuid(),

                PropertyTitle = req.PropertyTitle,
                Price = req.Price,
                PropertyType = req.PropertyType,
                PropertySubType = req.PropertySubType,
                IsLoanProviding = req.IsLoanProviding,

                PropertySqFt = req.PropertySqFt,
                PlotAreaSqYd = req.PlotAreaSqYd,
                PlotDimensions = req.PlotDimensions,
                TotalLandArea = req.TotalLandArea,
                PricePerAcre = req.PricePerAcre,
                Bedrooms = req.Bedrooms,
                Bathrooms = req.Bathrooms,
                NumberOfFloors = req.NumberOfFloors,
                FloorNumber = req.FloorNumber,
                FloorDetails = req.FloorDetails,
                MonthlyMaintenance = req.MonthlyMaintenance,
                Washrooms = req.Washrooms,
                CommercialType = req.CommercialType,
                LandType = req.LandType,
                GovApprovedCertificate = req.GovApprovedCertificate,

                FurnishingStatus = req.FurnishingStatus,
                FacingDirection = req.FacingDirection,
                AgeOfProperty = req.AgeOfProperty,

                HasSwimmingPool = req.HasSwimmingPool,
                HasGym = req.HasGym,
                HasSecurity = req.HasSecurity,
                HasParking = req.HasParking,
                HasClubHouse = req.HasClubHouse,
                HasPowerBackup = req.HasPowerBackup,
                HasLift = req.HasLift,
                HasGarden = req.HasGarden,
                HasKidsPlayArea = req.HasKidsPlayArea,
                HasCCTV = req.HasCCTV,
                HasIntercom = req.HasIntercom,
                HasFireSafety = req.HasFireSafety,
                HasWaterSupply24x7 = req.HasWaterSupply24x7,
                HasVisitorParking = req.HasVisitorParking,
                HasGatedCommunity = req.HasGatedCommunity,
                HasPartyHall = req.HasPartyHall,
                HasPark = req.HasPark,
                HasWalkingTrack = req.HasWalkingTrack,
                HasRainwaterHarvesting = req.HasRainwaterHarvesting,
                HasWasteManagement = req.HasWasteManagement,
                HasSeniorCitizenArea = req.HasSeniorCitizenArea,
                HasTerrace = req.HasTerrace,
                HasBalcony = req.HasBalcony,
                HasServantRoom = req.HasServantRoom,
                HasSolarPower = req.HasSolarPower,
                HasEVChargingPoint = req.HasEVChargingPoint,
                HasBlackTopRoad = req.HasBlackTopRoad,
                HasCornerPlot = req.HasCornerPlot,
                HasStreetLights = req.HasStreetLights,
                HasDrainageConnection = req.HasDrainageConnection,
                HasWaterConnection = req.HasWaterConnection,
                HasElectricityConnection = req.HasElectricityConnection,
                HasUndergroundSewage = req.HasUndergroundSewage,
                HasAvenueTrees = req.HasAvenueTrees,
                HasCompoundWall = req.HasCompoundWall,
                HasFencing = req.HasFencing,
                IsReadyForConstruction = req.IsReadyForConstruction,
                HasRoadAccess = req.HasRoadAccess,
                HasWaterSource = req.HasWaterSource,
                HasBorewell = req.HasBorewell,
                HasDripIrrigation = req.HasDripIrrigation,
                HasSprinklerSystem = req.HasSprinklerSystem,
                HasFarmHouse = req.HasFarmHouse,
                HasStorageShed = req.HasStorageShed,
                HasCattleShed = req.HasCattleShed,
                HasWatchmanRoom = req.HasWatchmanRoom,
                HasSolarPump = req.HasSolarPump,
                HasTreePlantation = req.HasTreePlantation,
                IsOrganicFarmingReady = req.IsOrganicFarmingReady,
                HasRiverAccess = req.HasRiverAccess,
                HasLakeView = req.HasLakeView,
                HasHillView = req.HasHillView,
                HasPrivateEntrance = req.HasPrivateEntrance,
                HasMunicipalityWaterSupply = req.HasMunicipalityWaterSupply,
                HasStoreRoom = req.HasStoreRoom,
                HasModularKitchen = req.HasModularKitchen,
                HasWiFi = req.HasWiFi,
                HasCentralizedAC = req.HasCentralizedAC,
                HasReceptionArea = req.HasReceptionArea,
                HasConferenceRoom = req.HasConferenceRoom,
                HasPantry = req.HasPantry,
                HasRestrooms = req.HasRestrooms,
                HasServiceLift = req.HasServiceLift,
                HasLoadingBay = req.HasLoadingBay,
                HasWheelchairAccess = req.HasWheelchairAccess,
                HasMaintenanceStaff = req.HasMaintenanceStaff,
                HasGeneratorBackup = req.HasGeneratorBackup,
                PropertyLoanPercentage = req.PropertyLoanPercentage,
                HospitalDistance = req.HospitalDistance,
                CollegeDistance = req.CollegeDistance,
                SchoolDistance = req.SchoolDistance,
                BusStandDistance = req.BusStandDistance,
                RailWayStationDistance = req.RailWayStationDistance,
                IsRental = req.IsRental,
                MonthlyRent = req.MonthlyRent,
                DepositAmount = req.DepositAmount,
                AvailableFrom = req.AvailableFrom,
                SharingType = req.SharingType,
                GenderAllowed = req.GenderAllowed,
                HasAttachedBathroom = req.HasAttachedBathroom,
                IsFurnished = req.IsFurnished,
                IsSale = req.IsSale,
                // PG AMENITIES
                HasFoodIncluded = req.HasFoodIncluded,
                HasWashingMachine = req.HasWashingMachine,
                HasHousekeeping = req.HasHousekeeping,
                HasBed = req.HasBed,
                HasCupboard = req.HasCupboard,
                HasTable = req.HasTable,
                HasChair = req.HasChair,
                HasAC = req.HasAC,
                HasTV = req.HasTV,
                HasGeyser = req.HasGeyser,
                HasSecurityGuard = req.HasSecurityGuard,
                HasSharedKitchen = req.HasSharedKitchen,
                IsCookingAllowed = req.IsCookingAllowed,

                // Save images as semi-colon separated string
                //ImageUrls = imageUrls.Any() ? string.Join(";", imageUrls) : "",


                ImageUrls = finalImages.Any() ? string.Join(";", finalImages) : "",

                VideoUrl1 = req.VideoUrl1,
                VideoUrl2 = req.VideoUrl2,
                Description = req.Description,
                Location = req.Location,
                LocationIframe = req.LocationIframe,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                IsActive = true
            };

            _context.PropertiesDetailsMaster.Add(property);
            await _context.SaveChangesAsync();

            //var audit = new PropertyAudit
            //{
            //    PropertyAuditId = Guid.NewGuid(),
            //    PropertyId = property.PropertiesDetailsId,
            //    FieldName = "Property",
            //    OldValue = null,
            //    NewValue = "Property Created",
            //    ActionType = "Add",
            //    ModifiedBy = userId,
            //    ModifiedOn = DateTime.UtcNow
            //};

            //_context.PropertyAudit.Add(audit);
            await _context.SaveChangesAsync();

            return (property.PropertiesDetailsId, "Property Created Successfully");
        }

        // ==========================
        // UPDATE PROPERTY
        // ==========================
        public async Task UpdateProperty(PropertyRequest req, Guid userId)
        {
            var oldProperty = await _context.PropertiesDetailsMaster
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PropertiesDetailsId == req.PropertiesDetailsId);

            if (oldProperty == null)
                throw new Exception("Property not found");

            var property = await _context.PropertiesDetailsMaster
                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == req.PropertiesDetailsId);

            if (property == null)
                throw new Exception("Property not found");

            //var imageUrls = await SaveImagesAsync(req.Images);

            var uploadedImages = await SaveImagesAsync(req.Images);
            var frontendImages = GetFrontendImages(req);

            // FULL UPDATE
            property.PropertyTitle = req.PropertyTitle;
            property.Price = req.Price;
            property.PropertyType = req.PropertyType;
            property.PropertySubType = req.PropertySubType;
            property.IsLoanProviding = req.IsLoanProviding;
            property.PropertyStatus = req.PropertyStatus;
            property.PropertySqFt = req.PropertySqFt;
            property.PlotAreaSqYd = req.PlotAreaSqYd;
            property.PlotDimensions = req.PlotDimensions;
            property.TotalLandArea = req.TotalLandArea;
            property.PricePerAcre = req.PricePerAcre;
            property.Bedrooms = req.Bedrooms;
            property.Bathrooms = req.Bathrooms;
            property.NumberOfFloors = req.NumberOfFloors;
            property.FloorNumber = req.FloorNumber;
            property.FloorDetails = req.FloorDetails;
            property.MonthlyMaintenance = req.MonthlyMaintenance;
            property.Washrooms = req.Washrooms;
            property.CommercialType = req.CommercialType;
            property.LandType = req.LandType;
            property.GovApprovedCertificate = req.GovApprovedCertificate;

            property.FurnishingStatus = req.FurnishingStatus;
            property.FacingDirection = req.FacingDirection;
            property.AgeOfProperty = req.AgeOfProperty;

            property.HasSwimmingPool = req.HasSwimmingPool;
            property.HasGym = req.HasGym;
            property.HasSecurity = req.HasSecurity;
            property.HasParking = req.HasParking;
            property.HasClubHouse = req.HasClubHouse;
            property.HasPowerBackup = req.HasPowerBackup;
            property.HasLift = req.HasLift;
            property.HasGarden = req.HasGarden;
            property.HasKidsPlayArea = req.HasKidsPlayArea;
            property.HasCCTV = req.HasCCTV;
            property.HasIntercom = req.HasIntercom;
            property.HasFireSafety = req.HasFireSafety;
            property.HasWaterSupply24x7 = req.HasWaterSupply24x7;
            property.HasVisitorParking = req.HasVisitorParking;
            property.HasGatedCommunity = req.HasGatedCommunity;
            property.HasPartyHall = req.HasPartyHall;
            property.HasPark = req.HasPark;
            property.HasWalkingTrack = req.HasWalkingTrack;
            property.HasRainwaterHarvesting = req.HasRainwaterHarvesting;
            property.HasWasteManagement = req.HasWasteManagement;
            property.HasSeniorCitizenArea = req.HasSeniorCitizenArea;
            property.HasTerrace = req.HasTerrace;
            property.HasBalcony = req.HasBalcony;
            property.HasServantRoom = req.HasServantRoom;
            property.HasSolarPower = req.HasSolarPower;
            property.HasEVChargingPoint = req.HasEVChargingPoint;
            property.HasBlackTopRoad = req.HasBlackTopRoad;
            property.HasCornerPlot = req.HasCornerPlot;
            property.HasStreetLights = req.HasStreetLights;
            property.HasDrainageConnection = req.HasDrainageConnection;
            property.HasWaterConnection = req.HasWaterConnection;
            property.HasElectricityConnection = req.HasElectricityConnection;
            property.HasUndergroundSewage = req.HasUndergroundSewage;
            property.HasAvenueTrees = req.HasAvenueTrees;
            property.HasCompoundWall = req.HasCompoundWall;
            property.HasFencing = req.HasFencing;
            property.IsReadyForConstruction = req.IsReadyForConstruction;
            property.HasRoadAccess = req.HasRoadAccess;
            property.HasWaterSource = req.HasWaterSource;
            property.HasBorewell = req.HasBorewell;
            property.HasDripIrrigation = req.HasDripIrrigation;
            property.HasSprinklerSystem = req.HasSprinklerSystem;
            property.HasFarmHouse = req.HasFarmHouse;
            property.HasStorageShed = req.HasStorageShed;
            property.HasCattleShed = req.HasCattleShed;
            property.HasWatchmanRoom = req.HasWatchmanRoom;
            property.HasSolarPump = req.HasSolarPump;
            property.HasTreePlantation = req.HasTreePlantation;
            property.IsOrganicFarmingReady = req.IsOrganicFarmingReady;
            property.HasRiverAccess = req.HasRiverAccess;
            property.HasLakeView = req.HasLakeView;
            property.HasHillView = req.HasHillView;
            property.HasPrivateEntrance = req.HasPrivateEntrance;
            property.HasMunicipalityWaterSupply = req.HasMunicipalityWaterSupply;
            property.HasStoreRoom = req.HasStoreRoom;
            property.HasModularKitchen = req.HasModularKitchen;
            property.HasWiFi = req.HasWiFi;
            property.HasCentralizedAC = req.HasCentralizedAC;
            property.HasReceptionArea = req.HasReceptionArea;
            property.HasConferenceRoom = req.HasConferenceRoom;
            property.HasPantry = req.HasPantry;
            property.HasRestrooms = req.HasRestrooms;
            property.HasServiceLift = req.HasServiceLift;
            property.HasLoadingBay = req.HasLoadingBay;
            property.HasWheelchairAccess = req.HasWheelchairAccess;
            property.HasMaintenanceStaff = req.HasMaintenanceStaff;
            property.HasGeneratorBackup = req.HasGeneratorBackup;
            property.PropertyLoanPercentage = req.PropertyLoanPercentage;
            property.HospitalDistance = req.HospitalDistance;
            property.CollegeDistance = req.CollegeDistance;
            property.SchoolDistance = req.SchoolDistance;
            property.BusStandDistance = req.BusStandDistance;
            property.RailWayStationDistance = req.RailWayStationDistance;
            property.IsRental = req.IsRental;
            property.MonthlyRent = req.MonthlyRent;
            property.DepositAmount = req.DepositAmount;
            property.AvailableFrom = req.AvailableFrom;
            property.SharingType = req.SharingType;
            property.GenderAllowed = req.GenderAllowed;
            property.HasAttachedBathroom = req.HasAttachedBathroom;
            property.IsFurnished = req.IsFurnished;
            property.IsSale = req.IsSale;
            // PG AMENITIES
            property.HasFoodIncluded = req.HasFoodIncluded;
            property.HasWashingMachine = req.HasWashingMachine;
            property.HasHousekeeping = req.HasHousekeeping;
            property.HasBed = req.HasBed;
            property.HasCupboard = req.HasCupboard;
            property.HasTable = req.HasTable;
            property.HasChair = req.HasChair;
            property.HasAC = req.HasAC;
            property.HasTV = req.HasTV;
            property.HasGeyser = req.HasGeyser;
            property.HasSecurityGuard = req.HasSecurityGuard;
            property.HasSharedKitchen = req.HasSharedKitchen;
            property.IsCookingAllowed = req.IsCookingAllowed;

            // Save images safely: new uploads > existing URLs from frontend > keep DB value
            //if (imageUrls.Any())
            //    property.ImageUrls = string.Join(";", imageUrls);
            //else if (!string.IsNullOrEmpty(req.ImageUrls))
            //    property.ImageUrls = req.ImageUrls;

            if (uploadedImages.Any())
            {
                property.ImageUrls = string.Join(";", uploadedImages);
            }
            else if (frontendImages.Any())
            {
                property.ImageUrls = string.Join(";", frontendImages);
            }
            else if (!string.IsNullOrEmpty(req.ImageUrls))
            {
                property.ImageUrls = req.ImageUrls;
            }

            property.VideoUrl1 = req.VideoUrl1;
            property.VideoUrl2 = req.VideoUrl2;
            property.Description = req.Description;
            property.Location = req.Location;
            property.IsActive = req.IsActive;
            property.UpdatedAt = DateTime.UtcNow;
            property.UpdatedBy = userId;

            //var audits = TrackAllChanges(oldProperty, property, userId);

            //if (audits.Any())
            //    await _context.PropertyAudit.AddRangeAsync(audits);

            await _context.SaveChangesAsync();
        }

        // ==========================
        // DELETE PROPERTY (SOFT DELETE)
        // ==========================
        public async Task DeleteProperty(Guid propertyId)
        {
            var property = await _context.PropertiesDetailsMaster
                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == propertyId);

            if (property == null)
                throw new Exception("Property not found");

            property.IsActive = false;
            property.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            //var audit = new PropertyAudit
            //{
            //    PropertyAuditId = Guid.NewGuid(),
            //    PropertyId = propertyId,
            //    FieldName = "Property",
            //    OldValue = "Active",
            //    NewValue = "Deleted",
            //    ActionType = "Delete",
            //    ModifiedBy = Guid.Empty, 
            //    ModifiedOn = DateTime.UtcNow
            //};

            //_context.PropertyAudit.Add(audit);
            await _context.SaveChangesAsync();
        }

        // ==========================
        // GET ALL PROPERTIES
        // ==========================
        public async Task<List<PropertyResponse>> GetAllPropertiesAsync()
        {
            var properties = await _context.PropertiesDetailsMaster.ToListAsync();

            //return properties.Select(p => new PropertyResponse
            return properties.Select(p =>
            {
                var imgs = GetImages(p.ImageUrls);
                return new PropertyResponse
                {
                    PropertiesDetailsId = p.PropertiesDetailsId,
                    PropertyTitle = p.PropertyTitle,
                    Price = p.Price ?? 0,
                    PropertyType = p.PropertyType,
                    PropertySubType = p.PropertySubType,
                    IsLoanProviding = p.IsLoanProviding,
                    PropertySqFt = p.PropertySqFt,
                    PlotAreaSqYd = p.PlotAreaSqYd,
                    PlotDimensions = p.PlotDimensions,
                    TotalLandArea = p.TotalLandArea,
                    PricePerAcre = p.PricePerAcre,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    NumberOfFloors = p.NumberOfFloors,
                    FloorNumber = p.FloorNumber,
                    FloorDetails = p.FloorDetails,
                    MonthlyMaintenance = p.MonthlyMaintenance,
                    Washrooms = p.Washrooms,
                    CommercialType = p.CommercialType,
                    LandType = p.LandType,
                    GovApprovedCertificate = p.GovApprovedCertificate,
                    FurnishingStatus = p.FurnishingStatus,
                    FacingDirection = p.FacingDirection,
                    AgeOfProperty = p.AgeOfProperty,
                    HasSwimmingPool = p.HasSwimmingPool ?? false,
                    HasGym = p.HasGym ?? false,
                    HasSecurity = p.HasSecurity ?? false,
                    HasParking = p.HasParking ?? false,
                    HasClubHouse = p.HasClubHouse ?? false,
                    HasPowerBackup = p.HasPowerBackup ?? false,
                    HasLift = p.HasLift ?? false,
                    HasGarden = p.HasGarden ?? false,
                    HasKidsPlayArea = p.HasKidsPlayArea ?? false,
                    HasCCTV = p.HasCCTV ?? false,
                    HasIntercom = p.HasIntercom ?? false,
                    HasFireSafety = p.HasFireSafety ?? false,
                    HasWaterSupply24x7 = p.HasWaterSupply24x7 ?? false,
                    HasVisitorParking = p.HasVisitorParking ?? false,
                    HasGatedCommunity = p.HasGatedCommunity ?? false,
                    HasPartyHall = p.HasPartyHall ?? false,
                    HasPark = p.HasPark ?? false,
                    HasWalkingTrack = p.HasWalkingTrack ?? false,
                    HasRainwaterHarvesting = p.HasRainwaterHarvesting ?? false,
                    HasWasteManagement = p.HasWasteManagement ?? false,
                    HasSeniorCitizenArea = p.HasSeniorCitizenArea ?? false,
                    HasTerrace = p.HasTerrace ?? false,
                    HasBalcony = p.HasBalcony ?? false,
                    HasServantRoom = p.HasServantRoom ?? false,
                    HasSolarPower = p.HasSolarPower ?? false,
                    HasEVChargingPoint = p.HasEVChargingPoint ?? false,
                    HasBlackTopRoad = p.HasBlackTopRoad ?? false,
                    HasCornerPlot = p.HasCornerPlot ?? false,
                    HasStreetLights = p.HasStreetLights ?? false,
                    HasDrainageConnection = p.HasDrainageConnection ?? false,
                    HasWaterConnection = p.HasWaterConnection ?? false,
                    HasElectricityConnection = p.HasElectricityConnection ?? false,
                    HasUndergroundSewage = p.HasUndergroundSewage ?? false,
                    HasAvenueTrees = p.HasAvenueTrees ?? false,
                    HasCompoundWall = p.HasCompoundWall ?? false,
                    HasFencing = p.HasFencing ?? false,
                    IsReadyForConstruction = p.IsReadyForConstruction ?? false,
                    HasRoadAccess = p.HasRoadAccess ?? false,
                    HasWaterSource = p.HasWaterSource ?? false,
                    HasBorewell = p.HasBorewell ?? false,
                    HasDripIrrigation = p.HasDripIrrigation ?? false,
                    HasSprinklerSystem = p.HasSprinklerSystem ?? false,
                    HasFarmHouse = p.HasFarmHouse ?? false,
                    HasStorageShed = p.HasStorageShed ?? false,
                    HasCattleShed = p.HasCattleShed ?? false,
                    HasWatchmanRoom = p.HasWatchmanRoom ?? false,
                    HasSolarPump = p.HasSolarPump ?? false,
                    HasTreePlantation = p.HasTreePlantation ?? false,
                    IsOrganicFarmingReady = p.IsOrganicFarmingReady ?? false,
                    HasRiverAccess = p.HasRiverAccess ?? false,
                    HasLakeView = p.HasLakeView ?? false,
                    HasHillView = p.HasHillView ?? false,
                    HasPrivateEntrance = p.HasPrivateEntrance ?? false,
                    HasMunicipalityWaterSupply = p.HasMunicipalityWaterSupply ?? false,
                    HasStoreRoom = p.HasStoreRoom ?? false,
                    HasModularKitchen = p.HasModularKitchen ?? false,
                    HasWiFi = p.HasWiFi ?? false,
                    HasCentralizedAC = p.HasCentralizedAC ?? false,
                    HasReceptionArea = p.HasReceptionArea ?? false,
                    HasConferenceRoom = p.HasConferenceRoom ?? false,
                    HasPantry = p.HasPantry ?? false,
                    HasRestrooms = p.HasRestrooms ?? false,
                    HasServiceLift = p.HasServiceLift ?? false,
                    HasLoadingBay = p.HasLoadingBay ?? false,
                    HasWheelchairAccess = p.HasWheelchairAccess ?? false,
                    HasMaintenanceStaff = p.HasMaintenanceStaff ?? false,
                    HasGeneratorBackup = p.HasGeneratorBackup ?? false,
                    PropertyLoanPercentage = p.PropertyLoanPercentage,
                    HospitalDistance = p.HospitalDistance,
                    CollegeDistance = p.CollegeDistance,
                    SchoolDistance = p.SchoolDistance,
                    BusStandDistance = p.BusStandDistance,
                    RailWayStationDistance = p.RailWayStationDistance,
                    IsRental = p.IsRental ?? false,
                    HasFoodIncluded = p.HasFoodIncluded ?? false,
                    HasWashingMachine = p.HasWashingMachine ?? false,
                    HasHousekeeping = p.HasHousekeeping ?? false,
                    HasBed = p.HasBed ?? false,
                    HasCupboard = p.HasCupboard ?? false,
                    HasTable = p.HasTable ?? false,
                    HasChair = p.HasChair ?? false,
                    HasAC = p.HasAC ?? false,
                    HasTV = p.HasTV ?? false,
                    HasGeyser = p.HasGeyser ?? false,
                    HasSecurityGuard = p.HasSecurityGuard ?? false,
                    HasSharedKitchen = p.HasSharedKitchen ?? false,
                    IsCookingAllowed = p.IsCookingAllowed ?? false,
                    MonthlyRent = p.MonthlyRent,
                    DepositAmount = p.DepositAmount,
                    AvailableFrom = p.AvailableFrom,
                    SharingType = p.SharingType,
                    GenderAllowed = p.GenderAllowed,
                    HasAttachedBathroom = p.HasAttachedBathroom ?? false,
                    IsFurnished = p.IsFurnished ?? false,
                    IsSale = p.IsSale ?? false,
                    //ImageUrls = string.IsNullOrEmpty(p.ImageUrls) ? new List<string>() : p.ImageUrls.Split(';').ToList(),
                    Img1 = imgs.Item1,
                    Img2 = imgs.Item2,
                    Img3 = imgs.Item3,
                    Img4 = imgs.Item4,
                    Img5 = imgs.Item5,
                    VideoUrl1 = p.VideoUrl1,
                    VideoUrl2 = p.VideoUrl2,
                    Description = p.Description,
                    Location = p.Location,
                    LocationIframe = p.LocationIframe,
                    PropertyStatus = p.PropertyStatus,
                    IsActive = p.IsActive,
                    CreatedBy = _context.UserMaster.FirstOrDefault(u => u.UserId == p.CreatedBy)?.Name ?? "Unknown",
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = _context.UserMaster.FirstOrDefault(u => u.UserId == p.UpdatedBy)?.Name ?? "Unknown",
                    UpdatedAt = p.UpdatedAt
                };
            }).ToList();
        }

        // ==========================
        // GET PROPERTY BY ID
        // ==========================
        public async Task<PropertyResponse> GetPropertyByIdAsync(Guid propertyId)
        {
            var property = await _context.PropertiesDetailsMaster
                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == propertyId);

            if (property == null) return null;

            var imgs = GetImages(property.ImageUrls);

            return new PropertyResponse
            {
                PropertiesDetailsId = property.PropertiesDetailsId,
                PropertyTitle = property.PropertyTitle,
                Price = property.Price ?? 0,
                PropertyType = property.PropertyType,
                PropertySubType = property.PropertySubType,
                IsLoanProviding = property.IsLoanProviding,
                PropertySqFt = property.PropertySqFt,
                PlotAreaSqYd = property.PlotAreaSqYd,
                PlotDimensions = property.PlotDimensions,
                TotalLandArea = property.TotalLandArea,
                PricePerAcre = property.PricePerAcre,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                NumberOfFloors = property.NumberOfFloors,
                FloorNumber = property.FloorNumber,
                FloorDetails = property.FloorDetails,
                MonthlyMaintenance = property.MonthlyMaintenance,
                Washrooms = property.Washrooms,
                CommercialType = property.CommercialType,
                LandType = property.LandType,
                GovApprovedCertificate = property.GovApprovedCertificate,
                FurnishingStatus = property.FurnishingStatus,
                FacingDirection = property.FacingDirection,
                AgeOfProperty = property.AgeOfProperty,
                HasSwimmingPool = property.HasSwimmingPool ?? false,
                HasGym = property.HasGym ?? false,
                HasSecurity = property.HasSecurity ?? false,
                HasParking = property.HasParking ?? false,
                HasClubHouse = property.HasClubHouse ?? false,
                HasPowerBackup = property.HasPowerBackup ?? false,
                HasLift = property.HasLift ?? false,
                HasGarden = property.HasGarden ?? false,
                HasKidsPlayArea = property.HasKidsPlayArea ?? false,
                HasCCTV = property.HasCCTV ?? false,
                HasIntercom = property.HasIntercom ?? false,
                HasFireSafety = property.HasFireSafety ?? false,
                HasWaterSupply24x7 = property.HasWaterSupply24x7 ?? false,
                HasVisitorParking = property.HasVisitorParking ?? false,
                HasGatedCommunity = property.HasGatedCommunity ?? false,
                HasPartyHall = property.HasPartyHall ?? false,
                HasPark = property.HasPark ?? false,
                HasWalkingTrack = property.HasWalkingTrack ?? false,
                HasRainwaterHarvesting = property.HasRainwaterHarvesting ?? false,
                HasWasteManagement = property.HasWasteManagement ?? false,
                HasSeniorCitizenArea = property.HasSeniorCitizenArea ?? false,
                HasTerrace = property.HasTerrace ?? false,
                HasBalcony = property.HasBalcony ?? false,
                HasServantRoom = property.HasServantRoom ?? false,
                HasSolarPower = property.HasSolarPower ?? false,
                HasEVChargingPoint = property.HasEVChargingPoint ?? false,
                HasBlackTopRoad = property.HasBlackTopRoad ?? false,
                HasCornerPlot = property.HasCornerPlot ?? false,
                HasStreetLights = property.HasStreetLights ?? false,
                HasDrainageConnection = property.HasDrainageConnection ?? false,
                HasWaterConnection = property.HasWaterConnection ?? false,
                HasElectricityConnection = property.HasElectricityConnection ?? false,
                HasUndergroundSewage = property.HasUndergroundSewage ?? false,
                HasAvenueTrees = property.HasAvenueTrees ?? false,
                HasCompoundWall = property.HasCompoundWall ?? false,
                HasFencing = property.HasFencing ?? false,
                IsReadyForConstruction = property.IsReadyForConstruction ?? false,
                HasRoadAccess = property.HasRoadAccess ?? false,
                HasWaterSource = property.HasWaterSource ?? false,
                HasBorewell = property.HasBorewell ?? false,
                HasDripIrrigation = property.HasDripIrrigation ?? false,
                HasSprinklerSystem = property.HasSprinklerSystem ?? false,
                HasFarmHouse = property.HasFarmHouse ?? false,
                HasStorageShed = property.HasStorageShed ?? false,
                HasCattleShed = property.HasCattleShed ?? false,
                HasWatchmanRoom = property.HasWatchmanRoom ?? false,
                HasSolarPump = property.HasSolarPump ?? false,
                HasTreePlantation = property.HasTreePlantation ?? false,
                IsOrganicFarmingReady = property.IsOrganicFarmingReady ?? false,
                HasRiverAccess = property.HasRiverAccess ?? false,
                HasLakeView = property.HasLakeView ?? false,
                HasHillView = property.HasHillView ?? false,
                HasPrivateEntrance = property.HasPrivateEntrance ?? false,
                HasMunicipalityWaterSupply = property.HasMunicipalityWaterSupply ?? false,
                HasStoreRoom = property.HasStoreRoom ?? false,
                HasModularKitchen = property.HasModularKitchen ?? false,
                HasWiFi = property.HasWiFi ?? false,
                HasCentralizedAC = property.HasCentralizedAC ?? false,
                HasReceptionArea = property.HasReceptionArea ?? false,
                HasConferenceRoom = property.HasConferenceRoom ?? false,
                HasPantry = property.HasPantry ?? false,
                HasRestrooms = property.HasRestrooms ?? false,
                HasServiceLift = property.HasServiceLift ?? false,
                HasLoadingBay = property.HasLoadingBay ?? false,
                HasWheelchairAccess = property.HasWheelchairAccess ?? false,
                HasMaintenanceStaff = property.HasMaintenanceStaff ?? false,
                HasGeneratorBackup = property.HasGeneratorBackup ?? false,
                PropertyLoanPercentage = property.PropertyLoanPercentage,
                HospitalDistance = property.HospitalDistance,
                CollegeDistance = property.CollegeDistance,
                SchoolDistance = property.SchoolDistance,
                BusStandDistance = property.BusStandDistance,
                RailWayStationDistance = property.RailWayStationDistance,
                IsRental = property.IsRental ?? false,
                HasFoodIncluded = property.HasFoodIncluded ?? false,
                HasWashingMachine = property.HasWashingMachine ?? false,
                HasHousekeeping = property.HasHousekeeping ?? false,
                HasBed = property.HasBed ?? false,
                HasCupboard = property.HasCupboard ?? false,
                HasTable = property.HasTable ?? false,
                HasChair = property.HasChair ?? false,
                HasAC = property.HasAC ?? false,
                HasTV = property.HasTV ?? false,
                HasGeyser = property.HasGeyser ?? false,
                HasSecurityGuard = property.HasSecurityGuard ?? false,
                HasSharedKitchen = property.HasSharedKitchen ?? false,
                IsCookingAllowed = property.IsCookingAllowed ?? false,
                MonthlyRent = property.MonthlyRent,
                DepositAmount = property.DepositAmount,
                AvailableFrom = property.AvailableFrom,
                SharingType = property.SharingType,
                GenderAllowed = property.GenderAllowed,
                HasAttachedBathroom = property.HasAttachedBathroom ?? false,
                IsFurnished = property.IsFurnished ?? false,
                IsSale = property.IsSale ?? false,
                //ImageUrls = string.IsNullOrEmpty(property.ImageUrls) ? new List<string>() : property.ImageUrls.Split(';').ToList(),
                Img1 = imgs.Item1,
                Img2 = imgs.Item2,
                Img3 = imgs.Item3,
                Img4 = imgs.Item4,
                Img5 = imgs.Item5,
                VideoUrl1 = property.VideoUrl1,
                VideoUrl2 = property.VideoUrl2,
                Description = property.Description,
                Location = property.Location,
                LocationIframe = property.LocationIframe,
                PropertyStatus = property.PropertyStatus,
                IsActive = property.IsActive,
                CreatedBy = _context.UserMaster.FirstOrDefault(u => u.UserId == property.CreatedBy)?.Name ?? "Unknown",
                CreatedAt = property.CreatedAt,
                UpdatedBy = _context.UserMaster.FirstOrDefault(u => u.UserId == property.UpdatedBy)?.Name ?? "Unknown",
                UpdatedAt = property.UpdatedAt

            };
        }

        public async Task<List<ActiveSoldOutPropertyResponse>> GetActiveAndSoldOutProperties()
        {
            var properties = await _context.PropertiesDetailsMaster
                .Where(p => p.IsActive &&
                       (p.PropertyStatus == "Active" || p.PropertyStatus == "Sold"))
                .Select(p => new ActiveSoldOutPropertyResponse
                {
                    PropertiesDetailsId = p.PropertiesDetailsId,
                    PropertyTitle = p.PropertyTitle,
                    Price = p.Price ?? 0,
                    PropertyType = p.PropertyType,
                    PropertyStatus = p.PropertyStatus,
                    Location = p.Location,
                    IsActive = p.IsActive,
                    CreatedBy = p.CreatedBy ?? Guid.Empty,
                    CreatedAt = p.CreatedAt,
                })
                .ToListAsync();

            return properties;
        }

        public async Task<PropertyDashboardResponse> GetPropertyDashboard(PropertyGraphRequest request)
        {
            // If year not provided use current year
            int year = request.Year ?? DateTime.UtcNow.Year;

            var list = await _context.PropertiesDetailsMaster
                .Where(p => p.IsActive && p.CreatedAt.Year == year)
                .ToListAsync();

            // GROUP MONTH-WISE
            var grouped = list
                .Where(p => p.CreatedAt != default && p.CreatedAt.Month >= 1 && p.CreatedAt.Month <= 12)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new PropertyGraphResponse
                {
                    Month = g.Key,
                    MonthName = new DateTime(year, g.Key, 1).ToString("MMM"),

                    ActiveCount = g.Count(x => x.PropertyStatus == "Active"),
                    SoldOutCount = g.Count(x => x.PropertyStatus == "Sold")
                })
                .ToList();

            // ALWAYS return all 12 months
            var graphData = Enumerable.Range(1, 12)
                .Select(m => grouped.FirstOrDefault(x => x.Month == m) ??
                    new PropertyGraphResponse
                    {
                        Month = m,
                        MonthName = new DateTime(year, m, 1).ToString("MMM"),
                        ActiveCount = 0,
                        SoldOutCount = 0
                    })
                .ToList();

            // SUMMARY
            var summary = new PropertySummaryResponse
            {
                TotalActive = list.Count(x => x.PropertyStatus == "Active"),
                TotalSoldOut = list.Count(x => x.PropertyStatus == "Sold")
            };

            return new PropertyDashboardResponse
            {
                Year = year,
                GraphData = graphData,
                Summary = summary
            };
        }

        public async Task<DashboardSummaryResponse> GeCountsandDistributionByProperties()
        {
            var properties = await _context.PropertiesDetailsMaster
                .Where(p => p.IsActive)
                .ToListAsync();

            var totalProperties = properties.Count;

            var activeCount = properties.Count(p => p.PropertyStatus == "Active");
            var soldCount = properties.Count(p => p.PropertyStatus == "Sold");

            // Leads 
            var totalLeads = await _context.CustomerContactMaster.CountAsync();

            // Distribution
            var distribution = properties
                .GroupBy(p => p.PropertyType)
                .Select(g => new PropertyTypeDistribution
                {
                    PropertyType = g.Key,
                    Count = g.Count(),
                    Percentage = totalProperties > 0
                        ? Math.Round((decimal)g.Count() * 100 / totalProperties, 2)
                        : 0
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return new DashboardSummaryResponse
            {
                TotalProperties = totalProperties,
                ActiveProperties = activeCount,
                SoldProperties = soldCount,
                TotalLeads = totalLeads,
                DistributionByProperties = distribution
            };
        }

        public async Task<List<PropertyAuditResponse>> GetPropertyAudit(Guid propertyId)
        {
            return await _context.PropertyAudit
                .Where(x => x.PropertyId == propertyId)
                .OrderByDescending(x => x.ModifiedOn)
                .Join(
                    _context.UserMaster,
                    audit => audit.ModifiedBy,
                    user => user.UserId,
                    (audit, user) => new PropertyAuditResponse
                    {
                        Description = audit.ActionType == "Update"
                            ? $"{audit.FieldName} changed from '{audit.OldValue}' to '{audit.NewValue}' by {user.Name} on {audit.ModifiedOn:dd-MMM-yyyy hh:mm tt}"
                            : $"{audit.ActionType} by {user.Name} on {audit.ModifiedOn:dd-MMM-yyyy hh:mm tt}",

                        Name = user.Name,
                        ModifiedOn = audit.ModifiedOn
                    }
                )
                .ToListAsync();
        }

        // ==========================
        // SAVE IMAGES
        // ==========================
        //private async Task<List<string>> SaveImagesAsync(IFormFileCollection images)
        //{
        //    var imageUrls = new List<string>();
        //    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "properties");

        //    if (!Directory.Exists(uploadPath))
        //        Directory.CreateDirectory(uploadPath);

        //    if (images == null || images.Count == 0)
        //        return imageUrls;

        //    foreach (var file in images)
        //    {
        //        if (file != null && file.Length > 0)
        //        {
        //            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //            var filePath = Path.Combine(uploadPath, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }

        //            imageUrls.Add($"/uploads/properties/{fileName}");
        //        }
        //    }

        //    return imageUrls;
        //}

        private async Task<List<string>> SaveImagesAsync(IFormFileCollection images)
        {
            var imageUrls = new List<string>();

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "properties");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            if (images == null || images.Count == 0)
                return imageUrls;

            foreach (var file in images)
            {
                if (file != null && file.Length > 0)
                {
                    // Unique file name
                    var fileName = $"{Guid.NewGuid()}.webp";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var image = await Image.LoadAsync(file.OpenReadStream()))
                    {
                        // Resize
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(1280, 1280) // max width/height
                        }));

                        // Save as WebP 
                        await image.SaveAsync(filePath, new WebpEncoder
                        {
                            Quality = 75 // 70–80 Quality
                        });
                    }

                    // Save relative path
                    imageUrls.Add($"/uploads/properties/{fileName}");
                }
            }

            return imageUrls;
        }


        // track changes for audit log - compares old vs new and creates entries for changed fields
        //private List<PropertyAudit> TrackAllChanges(
        //    PropertiesDetailsMaster oldData,
        //    PropertiesDetailsMaster newData,
        //    Guid userId)
        //{
        //    var audits = new List<PropertyAudit>();

        //    var properties = typeof(PropertiesDetailsMaster).GetProperties();

        //    foreach (var prop in properties)
        //    {
        //        // Skip unnecessary fields
        //        if (prop.Name == "CreatedAt" ||
        //            prop.Name == "CreatedBy" ||
        //            prop.Name == "UpdatedAt" ||
        //            prop.Name == "UpdatedBy")
        //            continue;

        //        var oldValue = prop.GetValue(oldData)?.ToString();
        //        var newValue = prop.GetValue(newData)?.ToString();

        //        if (oldValue != newValue)
        //        {
        //            audits.Add(new PropertyAudit
        //            {
        //                PropertyAuditId = Guid.NewGuid(),
        //                PropertyId = newData.PropertiesDetailsId,
        //                FieldName = FormatFieldName(prop.Name),
        //                OldValue = oldValue,
        //                NewValue = newValue,
        //                ActionType = "Update",
        //                ModifiedBy = userId,
        //                ModifiedOn = DateTime.UtcNow
        //            });
        //        }
        //    }

        //    return audits;
        //}

        private string FormatFieldName(string name)
        {
            return System.Text.RegularExpressions.Regex
                .Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        private List<string> GetFrontendImages(PropertyRequest req)
        {
            return new List<string>
            {
                req.Img1,
                req.Img2,
                req.Img3,
                req.Img4,
                req.Img5
            }
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        }

        private (string, string, string, string, string) GetImages(string imageUrls)
        {
            var images = string.IsNullOrEmpty(imageUrls)
                ? new List<string>()
                : imageUrls.Split(';').ToList();

            return (
                images.Count > 0 ? images[0] : "",
                images.Count > 1 ? images[1] : "",
                images.Count > 2 ? images[2] : "",
                images.Count > 3 ? images[3] : "",
                images.Count > 4 ? images[4] : ""
            );
        }
    }
}
