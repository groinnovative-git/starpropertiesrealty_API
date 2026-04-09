using Microsoft.EntityFrameworkCore;
using Star_Properties.DbConfiguration;
using Star_Properties.Model.EntityModel;
using Star_Properties.Model.ResponseModel;
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

        public async Task<UserMaster> GetUserByEmail(string email)
        {
            return await _context.UserMaster.FirstOrDefaultAsync(x => x.Email == email && x.IsActive);
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

        public async Task<bool> UpdateUserCrediential(Guid userId, string password, Guid modifiedBy)
        {
            var user = await _context.UserMaster
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            if (user == null)
                throw new Exception("User not found");

            user.Password = password; 
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
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

        public async Task TrackVisitor(string ip, string userAgent)
        {
            var today = DateTime.UtcNow.Date;

            // ✅ Avoid duplicate visitor (same IP same day)
            var exists = await _context.VisitorTracking
                .AnyAsync(x => x.IpAddress == ip && x.VisitedOn.Date == today);

            if (!exists)
            {
                var visitor = new VisitorTracking
                {
                    VisitorId = Guid.NewGuid(),
                    IpAddress = ip,
                    UserAgent = userAgent,
                    VisitedOn = DateTime.UtcNow
                };

                _context.VisitorTracking.Add(visitor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<VisitorDashboardResponse> GetVisitorDashboard()
        {
            var today = DateTime.UtcNow.Date;

            var total = await _context.VisitorTracking.CountAsync();

            var todayCount = await _context.VisitorTracking
                .Where(x => x.VisitedOn.Date == today)
                .CountAsync();

            var monthlyCount = await _context.VisitorTracking
                .Where(x => x.VisitedOn.Month == today.Month &&
                            x.VisitedOn.Year == today.Year)
                .CountAsync();

            return new VisitorDashboardResponse
            {
                TotalVisitors = total,
                TodayVisitors = todayCount,
                MonthlyVisitors = monthlyCount
            };
        }
    }
}
