using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.Repository.Interface.IPropertyRepository
{
    public interface IPropertyRepository
    {
        Task<Guid> AddProperty(PropertyRequest req, Guid userId);
        Task UpdateProperty(PropertyRequest req, Guid userId);
        Task DeleteProperty(Guid propertyId);
        Task<List<PropertyResponse>> GetAllPropertiesAsync();
        Task<PropertyResponse> GetPropertyByIdAsync(Guid propertyId);
        Task<List<ActiveSoldOutPropertyResponse>> GetActiveAndSoldOutProperties();
        Task<PropertyDashboardResponse> GetPropertyDashboard(PropertyGraphRequest request);
        Task<DashboardSummaryResponse> GeCountsandDistributionByProperties();
    }
}
