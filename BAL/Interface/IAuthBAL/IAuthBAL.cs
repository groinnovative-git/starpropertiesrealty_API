using Star_Properties.Model.EntityModel;
using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.BAL.Interface.IAuthBAL
{
    public interface IAuthBAL
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<string> CreateUserCrediential(CreateUserRequest request, Guid userId);
        Task<string> UpdateUserCrediential(UpdateUserRequest request, Guid userId);
        Task<string> DeleteUserCrediential(DeleteUserCredientialsRequest request, Guid userId);
        Task<List<UserMaster>> GetUserCrediential();
    }
}
