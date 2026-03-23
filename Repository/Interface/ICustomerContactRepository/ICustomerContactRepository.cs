using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.Repository.Interface.ICustomerContactRepository
{
    public interface ICustomerContactRepository
    {
        Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request);
    }
}
