using Star_Properties.Model.EntityModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.Repository.Interface.IAuthRepository
{
    public interface IAuthRepository
    {
        Task<UserMaster> GetUserByUsername(string username);
        Task<UserMaster> GetUserByEmail(string email);
        Task<UserMaster> GetUserByUserId(Guid userId);
        Task CreateUserCrediential(UserMaster user);
        Task<bool> UpdateUserCrediential(Guid userId, string password, Guid modifiedBy);
        Task<string> DeleteUserCrediential(Guid deleteUserId, Guid modifiedBy);
        Task<List<UserMaster>> GetUserCrediential();
        Task TrackVisitor(string ip, string userAgent);
        Task<VisitorDashboardResponse> GetVisitorDashboard();
    }
}
