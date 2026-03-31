using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.Repository.Interface.ICustomerContactRepository
{
    public interface ICustomerContactRepository
    {
        Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request);
        Task<List<CustomerContactResponse>> GetAllContacts();
        Task<CustomerContactResponse> UpdateContact(CustomerContactRequest request, Guid userId);
        Task<List<CustomerContactAuditResponse>> GetContactAuditDetails(Guid contactId);
    }
}
