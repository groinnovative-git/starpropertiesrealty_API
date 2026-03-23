using Star_Properties.Model.RequestModel;
using Star_Properties.Model.ResponseModel;

namespace Star_Properties.BAL.Interface.IAuthBAL
{
    public interface IAuthBAL
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}
