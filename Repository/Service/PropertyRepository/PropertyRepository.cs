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

            // Save images safely
            property.ImageUrls = imageUrls.Any() ? string.Join(";", imageUrls) : property.ImageUrls;

            property.VideoUrl1 = req.VideoUrl1;
            property.VideoUrl2 = req.VideoUrl2;
            property.Description = req.Description;
            property.Location = req.Location;

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
                ImageUrls = string.IsNullOrEmpty(p.ImageUrls) ? new List<string>() : p.ImageUrls.Split(';').ToList(),
                VideoUrl1 = p.VideoUrl1,
                VideoUrl2 = p.VideoUrl2,
                Description = p.Description,
                Location = p.Location
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
                ImageUrls = string.IsNullOrEmpty(property.ImageUrls) ? new List<string>() : property.ImageUrls.Split(';').ToList(),
                VideoUrl1 = property.VideoUrl1,
                VideoUrl2 = property.VideoUrl2,
                Description = property.Description,
                Location = property.Location
            };
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








//using Microsoft.EntityFrameworkCore;
//using Star_Properties.DbConfiguration;
//using Star_Properties.Model.EntityModel;
//using Star_Properties.Model.RequestModel;
//using Star_Properties.Model.ResponseModel;
//using Star_Properties.Repository.Interface.IPropertyRepository;
//using System.Text.Json;

//namespace Star_Properties.Repository.Service.PropertyRepository
//{
//    public class PropertyRepository : IPropertyRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public PropertyRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Guid> AddProperty(PropertyRequest req, Guid userId)
//        {
//            var imageUrls = await SaveImagesAsync(req.Images);

//            var property = new PropertiesDetailsMaster
//            {
//                PropertiesDetailsId = Guid.NewGuid(),

//                PropertyTitle = req.PropertyTitle,
//                Price = req.Price,
//                PropertyType = req.PropertyType,
//                PropertySubType = req.PropertySubType,
//                IsLoanProviding = req.IsLoanProviding,

//                PropertySqFt = req.PropertySqFt,
//                PlotAreaSqYd = req.PlotAreaSqYd,
//                Bedrooms = req.Bedrooms,
//                Bathrooms = req.Bathrooms,
//                NumberOfFloors = req.NumberOfFloors,
//                FloorNumber = req.FloorNumber,
//                MonthlyMaintenance = req.MonthlyMaintenance,
//                Washrooms = req.Washrooms,
//                CommercialType = req.CommercialType,
//                IsGovApproved = req.IsGovApproved,

//                FurnishingStatus = req.FurnishingStatus,
//                FacingDirection = req.FacingDirection,
//                AgeOfProperty = req.AgeOfProperty,

//                HasSwimmingPool = req.HasSwimmingPool,
//                HasGym = req.HasGym,
//                HasSecurity = req.HasSecurity,
//                HasParking = req.HasParking,
//                HasClubHouse = req.HasClubHouse,
//                HasPowerBackup = req.HasPowerBackup,
//                HasLift = req.HasLift,
//                HasGarden = req.HasGarden,
//                HasKidsPlayArea = req.HasKidsPlayArea,
//                HasCCTV = req.HasCCTV,
//                HasIntercom = req.HasIntercom,
//                HasFireSafety = req.HasFireSafety,
//                HasWaterSupply24x7 = req.HasWaterSupply24x7,

//                //ImageUrls = JsonSerializer.Serialize(imageUrls),
//                ImageUrls = string.Join(";", imageUrls),
//                VideoUrl1 = req.VideoUrl1,
//                VideoUrl2 = req.VideoUrl2,

//                Description = req.Description,
//                Location = req.Location,
//                LocationIframe = req.LocationIframe,

//                CreatedAt = DateTime.UtcNow,
//                CreatedBy = userId,
//                IsActive = true
//            };

//            _context.PropertiesDetailsMaster.Add(property);
//            await _context.SaveChangesAsync();

//            return property.PropertiesDetailsId;
//        }

//        public async Task UpdateProperty(PropertyRequest req, Guid userId)
//        {
//            var property = await _context.PropertiesDetailsMaster
//                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == req.PropertiesDetailsId);

//            var imageUrls = await SaveImagesAsync(req.Images);

//            if (property == null)
//                throw new Exception("Property not found");

//            // FULL UPDATE
//            property.PropertyTitle = req.PropertyTitle;
//            property.Price = req.Price;
//            property.PropertyType = req.PropertyType;
//            property.PropertySubType = req.PropertySubType;
//            property.IsLoanProviding = req.IsLoanProviding;

//            property.PropertySqFt = req.PropertySqFt;
//            property.PlotAreaSqYd = req.PlotAreaSqYd;
//            property.Bedrooms = req.Bedrooms;
//            property.Bathrooms = req.Bathrooms;
//            property.NumberOfFloors = req.NumberOfFloors;
//            property.FloorNumber = req.FloorNumber;
//            property.MonthlyMaintenance = req.MonthlyMaintenance;
//            property.Washrooms = req.Washrooms;
//            property.CommercialType = req.CommercialType;
//            property.IsGovApproved = req.IsGovApproved;

//            property.FurnishingStatus = req.FurnishingStatus;
//            property.FacingDirection = req.FacingDirection;
//            property.AgeOfProperty = req.AgeOfProperty;

//            property.HasSwimmingPool = req.HasSwimmingPool;
//            property.HasGym = req.HasGym;
//            property.HasSecurity = req.HasSecurity;
//            property.HasParking = req.HasParking;
//            property.HasClubHouse = req.HasClubHouse;
//            property.HasPowerBackup = req.HasPowerBackup;
//            property.HasLift = req.HasLift;
//            property.HasGarden = req.HasGarden;
//            property.HasKidsPlayArea = req.HasKidsPlayArea;
//            property.HasCCTV = req.HasCCTV;
//            property.HasIntercom = req.HasIntercom;
//            property.HasFireSafety = req.HasFireSafety;
//            property.HasWaterSupply24x7 = req.HasWaterSupply24x7;
//            //property.ImageUrls = JsonSerializer.Serialize(imageUrls);
//            property.ImageUrls = string.Join(";", imageUrls);
//            property.VideoUrl1 = req.VideoUrl1;
//            property.VideoUrl2 = req.VideoUrl2;

//            property.Description = req.Description;
//            property.Location = req.Location;

//            property.UpdatedAt = DateTime.UtcNow;
//            property.UpdatedBy = userId;

//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteProperty(Guid propertyId)
//        {
//            var property = await _context.PropertiesDetailsMaster
//                .FirstOrDefaultAsync(x => x.PropertiesDetailsId == propertyId);

//            if (property == null)
//                throw new Exception("Property not found");

//            property.IsActive = false; 
//            property.UpdatedAt = DateTime.UtcNow;

//            await _context.SaveChangesAsync();
//        }

//        public async Task<List<PropertyResponse>> GetAllPropertiesAsync()
//        {
//            var properties = await _context.PropertiesDetailsMaster.ToListAsync();

//            return properties.Select(p => new PropertyResponse
//            {
//                    PropertiesDetailsId = p.PropertiesDetailsId,
//                    PropertyTitle = p.PropertyTitle,
//                    Price = p.Price ?? 0,
//                    PropertyType = p.PropertyType, 
//                    PropertySubType = p.PropertySubType,
//                    IsLoanProviding = p.IsLoanProviding,
//                    PropertySqFt = p.PropertySqFt,
//                    PlotAreaSqYd = p.PlotAreaSqYd,
//                    Bedrooms = p.Bedrooms,
//                    Bathrooms = p.Bathrooms,
//                    NumberOfFloors = p.NumberOfFloors,
//                    FloorNumber = p.FloorNumber,
//                    MonthlyMaintenance = p.MonthlyMaintenance,
//                    Washrooms = p.Washrooms,
//                    CommercialType = p.CommercialType,
//                    IsGovApproved = p.IsGovApproved,
//                    FurnishingStatus = p.FurnishingStatus,
//                    FacingDirection = p.FacingDirection,
//                    AgeOfProperty = p.AgeOfProperty,
//                    HasSwimmingPool = p.HasSwimmingPool ?? false,
//                    HasGym = p.HasGym ?? false,
//                    HasSecurity = p.HasSecurity ?? false,
//                    HasParking = p.HasParking ?? false,
//                    HasClubHouse = p.HasClubHouse ?? false,
//                    HasPowerBackup = p.HasPowerBackup ?? false,
//                    HasLift = p.HasLift ?? false,
//                    HasGarden = p.HasGarden ?? false,
//                    HasKidsPlayArea = p.HasKidsPlayArea ?? false,
//                    HasCCTV = p.HasCCTV ?? false,
//                    HasIntercom = p.HasIntercom ?? false,
//                    HasFireSafety = p.HasFireSafety ?? false,
//                    HasWaterSupply24x7 = p.HasWaterSupply24x7 ?? false,
//                    ImageUrls = string.IsNullOrEmpty(p.ImageUrls) ? new List<string>() : p.ImageUrls.Split(';').ToList(),
//                    VideoUrl1 = p.VideoUrl1,
//                    VideoUrl2 = p.VideoUrl2,
//                    Description = p.Description,
//                    Location = p.Location
//                }).ToList();
//        }

//        public async Task<PropertyResponse> GetPropertyByIdAsync(Guid propertyId)
//        {
//            var property = await _context.PropertiesDetailsMaster.FirstOrDefaultAsync(x => x.PropertiesDetailsId == propertyId);
//            if (property == null) return null;

//            return new PropertyResponse
//            {
//                PropertiesDetailsId = property.PropertiesDetailsId,
//                PropertyTitle = property.PropertyTitle,
//                Price = property.Price ?? 0,
//                PropertyType = property.PropertyType,
//                PropertySubType = property.PropertySubType,
//                IsLoanProviding = property.IsLoanProviding,
//                PropertySqFt = property.PropertySqFt,
//                PlotAreaSqYd = property.PlotAreaSqYd,
//                Bedrooms = property.Bedrooms,
//                Bathrooms = property.Bathrooms,
//                NumberOfFloors = property.NumberOfFloors,
//                FloorNumber = property.FloorNumber,
//                MonthlyMaintenance = property.MonthlyMaintenance,
//                Washrooms = property.Washrooms,
//                CommercialType = property.CommercialType,
//                IsGovApproved = property.IsGovApproved,
//                FurnishingStatus = property.FurnishingStatus,
//                FacingDirection = property.FacingDirection,
//                AgeOfProperty = property.AgeOfProperty,
//                HasSwimmingPool = property.HasSwimmingPool ?? false,
//                HasGym = property.HasGym ?? false,
//                HasSecurity = property.HasSecurity ?? false,
//                HasParking = property.HasParking ?? false,
//                HasClubHouse = property.HasClubHouse ?? false,
//                HasPowerBackup = property.HasPowerBackup ?? false,
//                HasLift = property.HasLift ?? false,
//                HasGarden = property.HasGarden ?? false,
//                HasKidsPlayArea = property.HasKidsPlayArea ?? false,
//                HasCCTV = property.HasCCTV ?? false,
//                HasIntercom = property.HasIntercom ?? false,
//                HasFireSafety = property.HasFireSafety ?? false,
//                HasWaterSupply24x7 = property.HasWaterSupply24x7 ?? false,
//                //ImageUrls = property.ImageUrls?.Split(';').ToList() ?? new List<string>(),
//                ImageUrls = string.IsNullOrEmpty(property.ImageUrls) ? new List<string>() : property.ImageUrls.Split(';').ToList(),
//                VideoUrl1 = property.VideoUrl1,
//                VideoUrl2 = property.VideoUrl2,
//                Description = property.Description,
//                Location = property.Location
//            };
//        }

//        private async Task<List<string>> SaveImagesAsync(IFormFileCollection images)
//        {
//            var imageUrls = new List<string>();
//            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "properties");

//            if (!Directory.Exists(uploadPath))
//                Directory.CreateDirectory(uploadPath);

//            foreach (var file in images)
//            {
//                if (file.Length > 0)
//                {
//                    // Compress or resize if needed
//                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
//                    var filePath = Path.Combine(uploadPath, fileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await file.CopyToAsync(stream);
//                    }

//                    imageUrls.Add($"/uploads/properties/{fileName}"); 
//                }
//            }

//            return imageUrls;
//        }
//    }
//}
