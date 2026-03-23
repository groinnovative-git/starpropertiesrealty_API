using Star_Properties.BAL.Interface.IPropertyBAL;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.IPropertyRepository;

namespace Star_Properties.BAL.Service.PropertyBAL
{
    public class PropertyBAL : IPropertyBAL
    {
        private readonly IPropertyRepository _repo;

        public PropertyBAL(IPropertyRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> AddProperty(PropertyRequest req, Guid userId)
        {
            try
            {
                var result = _repo.AddProperty(req, userId);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
           

        public Task UpdateProperty(PropertyRequest req, Guid userId)
        {
            try
            {
                var result = _repo.UpdateProperty(req, userId);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task DeleteProperty(Guid propertyId)
        {
            try
            {
                var result = _repo.DeleteProperty(propertyId);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PropertyResponse>> GetAllProperties()
        {
            try
            {
                //return await _repo.GetAllPropertiesAsync();
                var result = await _repo.GetAllPropertiesAsync();
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PropertyResponse> GetPropertiesById(Guid propertyId)
        {
            try
            {
                //return await _repo.GetPropertyByIdAsync(id);
                var result = await _repo.GetPropertyByIdAsync(propertyId);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
