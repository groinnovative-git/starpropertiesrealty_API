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

        public async Task<UserMaster> GetUserByUsername(string username)
        {
            return await _context.UserMaster.FirstOrDefaultAsync(x => x.Username == username && x.IsActive);
        }

        public async Task<UserMaster> GetUserByUserId(Guid userId)
        {
            return await _context.UserMaster.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);
        }

        public async Task CreateUserCrediential(UserMaster user)
        {
            await _context.UserMaster.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserCrediential(UserMaster user)
        {
            _context.UserMaster.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteUserCrediential(Guid deleteUserId, Guid modifiedBy)
        {
            var user = await _context.UserMaster
                .FirstOrDefaultAsync(x => x.UserId == deleteUserId);

            if (user == null)
                return "User Not Found";

            user.IsActive = false;
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "User Deleted Successfully";
        }

        public async Task<List<UserMaster>> GetUserCrediential()
        {
            return await _context.UserMaster
                .Where(x => x.IsActive)
                .ToListAsync();
        }
    }
}
