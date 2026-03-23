using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.BAL.Interface.IPropertyBAL
{
    public interface IPropertyBAL
    {
        Task<Guid> AddProperty(PropertyRequest req, Guid userId);
        Task UpdateProperty(PropertyRequest req, Guid userId);
        Task DeleteProperty(Guid propertyId);
        Task<List<PropertyResponse>> GetAllProperties();
        Task<PropertyResponse> GetPropertiesById(Guid propertyId);
    }
}
