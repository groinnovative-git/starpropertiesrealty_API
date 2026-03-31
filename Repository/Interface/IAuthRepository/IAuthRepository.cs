using Star_Properties.Model.EntityModel;

namespace Star_Properties.Repository.Interface.IAuthRepository
{
    public interface IAuthRepository
    {
        Task<UserMaster> GetUserByUsername(string username);
        Task<UserMaster> GetUserByUserId(Guid userId);
        Task CreateUserCrediential(UserMaster user);
        Task UpdateUserCrediential(UserMaster user);
        Task<string> DeleteUserCrediential(Guid deleteUserId, Guid modifiedBy);
        Task<List<UserMaster>> GetUserCrediential()    }
}
