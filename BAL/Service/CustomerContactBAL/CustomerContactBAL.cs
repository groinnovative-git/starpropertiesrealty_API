using Star_Properties.BAL.Interface.ICustomerContactBAL;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;
using Star_Properties.Repository.Interface.ICustomerContactRepository;

namespace Star_Properties.BAL.Service.CustomerContactBAL
{
    public class CustomerContactBAL : ICustomerContactBAL
    {
        private readonly ICustomerContactRepository _repo;
        public CustomerContactBAL(ICustomerContactRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request)
        {
            return await _repo.SubmitContact(request);
        }

        public async Task<List<CustomerContactResponse>> GetAllContacts()
        {
            return await _repo.GetAllContacts();
        }

        public async Task<CustomerContactResponse> UpdateContact(CustomerContactRequest request, Guid userId)
        {
            return await _repo.UpdateContact(request, userId);
        }

        public async Task<List<CustomerContactAuditResponse>> GetContactAuditDetails(Guid contactId)
        {
            return await _repo.GetContactAuditDetails(contactId);
        }
    }
}
