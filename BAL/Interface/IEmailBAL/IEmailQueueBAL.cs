using Star_Properties.Model.RequestModel;

namespace Star_Properties.BAL.Interface.IEmailBAL
{
    public interface IEmailQueueBAL
    {
        public void Enqueue(SendEmailRequest request);
    }
}
