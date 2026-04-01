using Star_Properties.Model.RequestModel;

namespace Star_Properties.BAL.Interface.IEmailBAL
{
    public interface IEmailBAL
    {
        //Task<bool> SendEmail(SendEmailRequest request);
        Task<bool> SendEmail(SendEmailRequest request, HttpContext httpContext);
    }
}
