using Star_Properties.Model.EntityModel;

namespace Star_Properties.Repository.Interface.IAuthRepository
{
    public interface IAuthRepository
    {
        Task<UserMaster> GetUserByEmailAsync(string email);
    }
}
