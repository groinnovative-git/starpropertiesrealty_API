using Microsoft.EntityFrameworkCore;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.IPropertyRepository;

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
        public async Task<Guid> AddProperty(PropertyRequest req, Guid userId)
        {
            var imageUrls = await SaveImagesAsync(req.Images);

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
                Bedrooms = req.Bedrooms,
                Bathrooms = req.Bathrooms,
                NumberOfFloors = req.NumberOfFloors,
                FloorNumber = req.FloorNumber,
                MonthlyMaintenance = req.MonthlyMaintenance,
                Washrooms = req.Washrooms,
                CommercialType = req.CommercialType,
                IsGovApproved = req.IsGovApproved,

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

                // Save images as semi-colon separated string
                ImageUrls = imageUrls.Any() ? string.Join(";", imageUrls) : "",

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

            return property.PropertiesDetailsId;
        }

        // ==========================
        // UPDATE PROPERTY
        // ==========================
        public async Task UpdateProperty(PropertyRequest req, Guid userId)
        {
            var property = await _context.PropertiesDetailsMaster
                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == req.PropertiesDetailsId);

            if (property == null)
                throw new Exception("Property not found");

            var imageUrls = await SaveImagesAsync(req.Images);

            // FULL UPDATE
            property.PropertyTitle = req.PropertyTitle;
            property.Price = req.Price;
            property.PropertyType = req.PropertyType;
            property.PropertySubType = req.PropertySubType;
            property.IsLoanProviding = req.IsLoanProviding;
            property.PropertyStatus = req.PropertyStatus;
            property.PropertySqFt = req.PropertySqFt;
            property.PlotAreaSqYd = req.PlotAreaSqYd;
            property.Bedrooms = req.Bedrooms;
            property.Bathrooms = req.Bathrooms;
            property.NumberOfFloors = req.NumberOfFloors;
            property.FloorNumber = req.FloorNumber;
            property.MonthlyMaintenance = req.MonthlyMaintenance;
            property.Washrooms = req.Washrooms;
            property.CommercialType = req.CommercialType;
            property.IsGovApproved = req.IsGovApproved;

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

            // Save images safely: new uploads > existing URLs from frontend > keep DB value
            if (imageUrls.Any())
                property.ImageUrls = string.Join(";", imageUrls);
            else if (!string.IsNullOrEmpty(req.ImageUrls))
                property.ImageUrls = req.ImageUrls;

            property.VideoUrl1 = req.VideoUrl1;
            property.VideoUrl2 = req.VideoUrl2;
            property.Description = req.Description;
            property.Location = req.Location;
            property.IsActive = req.IsActive;
            property.UpdatedAt = DateTime.UtcNow;
            property.UpdatedBy = userId;

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
        }

        // ==========================
        // GET ALL PROPERTIES
        // ==========================
        public async Task<List<PropertyResponse>> GetAllPropertiesAsync()
        {
            var properties = await _context.PropertiesDetailsMaster.ToListAsync();

            return properties.Select(p => new PropertyResponse
            {
                PropertiesDetailsId = p.PropertiesDetailsId,
                PropertyTitle = p.PropertyTitle,
                Price = p.Price ?? 0,
                PropertyType = p.PropertyType,
                PropertySubType = p.PropertySubType,
                IsLoanProviding = p.IsLoanProviding,
                PropertySqFt = p.PropertySqFt,
                PlotAreaSqYd = p.PlotAreaSqYd,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                NumberOfFloors = p.NumberOfFloors,
                FloorNumber = p.FloorNumber,
                MonthlyMaintenance = p.MonthlyMaintenance,
                Washrooms = p.Washrooms,
                CommercialType = p.CommercialType,
                IsGovApproved = p.IsGovApproved,
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
                ImageUrls = string.IsNullOrEmpty(p.ImageUrls) ? new List<string>() : p.ImageUrls.Split(';').ToList(),
                VideoUrl1 = p.VideoUrl1,
                VideoUrl2 = p.VideoUrl2,
                Description = p.Description,
                Location = p.Location,
                LocationIframe = p.LocationIframe,
                PropertyStatus = p.PropertyStatus,
                IsActive = p.IsActive,

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
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                NumberOfFloors = property.NumberOfFloors,
                FloorNumber = property.FloorNumber,
                MonthlyMaintenance = property.MonthlyMaintenance,
                Washrooms = property.Washrooms,
                CommercialType = property.CommercialType,
                IsGovApproved = property.IsGovApproved,
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
                ImageUrls = string.IsNullOrEmpty(property.ImageUrls) ? new List<string>() : property.ImageUrls.Split(';').ToList(),
                VideoUrl1 = property.VideoUrl1,
                VideoUrl2 = property.VideoUrl2,
                Description = property.Description,
                Location = property.Location,
                 LocationIframe = property.LocationIframe,
                PropertyStatus = property.PropertyStatus,
               IsActive = property.IsActive,

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

        // ==========================
        // SAVE IMAGES
        // ==========================
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
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imageUrls.Add($"/uploads/properties/{fileName}");
                }
            }

            return imageUrls;
        }
    }
}
