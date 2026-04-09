using Star_Properties.BAL.Interface.IEmailBAL;
using Star_Properties.Model.RequestModel;
using System.Collections.Concurrent;

namespace Star_Properties.BAL.Service.EmailBAL
{
    public class EmailQueueBAL : BackgroundService, IEmailQueueBAL
    {
        private readonly ConcurrentQueue<SendEmailRequest> _queue = new();
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailQueueBAL(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Enqueue(SendEmailRequest request)
        {
            _queue.Enqueue(request);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var request))
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailBAL = scope.ServiceProvider.GetRequiredService<IEmailBAL>();

                    try
                    {
                        await emailBAL.SendEmail(request, null);
                    }
                    catch (Exception ex)
                    {
                        // 🔴 log error (important)
                    }
                }

                await Task.Delay(500, stoppingToken); 
            }
        }
    }
}
