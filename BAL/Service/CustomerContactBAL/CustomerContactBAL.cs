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

        //public async Task<CustomerContactResponse> UpdateContact(CustomerContactRequest request, Guid userId)
        //{
        //    return await _repo.UpdateContact(request, userId);
        //}


        public async Task<CustomerContactUpdateResponse> UpdateContact(CustomerContactRequest request, Guid userId)
        {
            return await _repo.UpdateContact(request, userId);
        }

        public async Task<List<CustomerContactAuditResponse>> GetContactAuditDetails(Guid contactId)
        {
            return await _repo.GetContactAuditDetails(contactId);
        }

        public async Task CreateNotification(string title, string message)
        {
            await _repo.CreateNotification(title, message);
        }

        public async Task<List<NotificationResponse>> GetNotifications(Guid userId)
        {
            return await _repo.GetNotifications(userId);
        }

        public async Task MarkAsReadBulk(List<Guid> notificationIds, Guid userId)
        {
            await _repo.MarkAsReadBulk(notificationIds, userId);
        }

        public async Task<int> GetUnreadCount(Guid userId)
        {
            return await _repo.GetUnreadCount(userId);
        }
    }
}
