using Microsoft.EntityFrameworkCore;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Repository.Interface.IAuthRepository;
using System;

namespace Star_Properties.Repository.Service.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserMaster> GetUserByEmailAsync(string email)
        {
            return await _context.UserMaster.FirstOrDefaultAsync(x => x.Email == email && x.IsActive);
        }
    }
}
