using Star_Properties.Model.EntityModel;

namespace Star_Properties.Repository.Interface.IEmailRepository
{
    public interface IEmailRepository
    {
        Task SaveEmailLog(EmailLog log);
    }
}
