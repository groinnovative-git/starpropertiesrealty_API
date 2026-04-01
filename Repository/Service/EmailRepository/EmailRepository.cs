using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Repository.Interface.IEmailRepository;
using System;

namespace Star_Properties.Repository.Service.EmailRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveEmailLog(EmailLog log)
        {
            _context.EmailLog.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
