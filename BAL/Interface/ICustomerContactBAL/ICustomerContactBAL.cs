using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.BAL.Interface.ICustomerContactBAL
{
    public interface ICustomerContactBAL
    {
        Task<CustomerContactResponse> SubmitContact(CustomerContactRequest request);
        Task<List<CustomerContactResponse>> GetAllContacts();
        //Task<CustomerContactResponse> UpdateContact(CustomerContactRequest request, Guid userId);
        Task<CustomerContactUpdateResponse> UpdateContact(CustomerContactRequest request, Guid userId);
        Task<List<CustomerContactAuditResponse>> GetContactAuditDetails(Guid contactId);
        Task CreateNotification(string title, string message);
        Task<List<NotificationResponse>> GetNotifications(Guid userId);
        Task MarkAsReadBulk(List<Guid> notificationIds, Guid userId);
        Task<int> GetUnreadCount(Guid userId);
    }
}
