using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.BAL.Interface.ICustomerContactBAL
{
    public interface ICustomerContactBAL
    {
        Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request);
    }
}
