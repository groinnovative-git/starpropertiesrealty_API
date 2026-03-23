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
    }
}
