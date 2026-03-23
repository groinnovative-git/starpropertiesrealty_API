using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.ICustomerContactRepository;

namespace Star_Properties.Repository.Service.CustomerContactRepository
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request)
        {
            var entity = new CustomerContactMaster
            {
                ContactId = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CustomerInterest = request.CustomerInterest,
                Message = request.Message,
                PropertyId = request.PropertyId,
                SubmittedDate = DateTime.UtcNow
            };

            _context.CustomerContactMaster.Add(entity);
            await _context.SaveChangesAsync();

            return new CustomerContactResponse
            {
                ContactId = entity.ContactId,
                FullName = entity.FullName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                CustomerInterest = entity.CustomerInterest,
                Message = entity.Message,
                PropertyId = entity.PropertyId,
                SubmittedDate = entity.SubmittedDate
            };
        }
    }
}
