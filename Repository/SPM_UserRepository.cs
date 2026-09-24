using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Repository
{
    public class SPM_UserRepository : ISPM_UserRepository
    {
        private readonly AppDbContext _context;

        public SPM_UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SPM_User>> GetAllAsync()
        {
            return await _context.SPM_Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<SPM_User?> GetByIdAsync(int id)
        {
            return await _context.SPM_Users
                .FirstOrDefaultAsync(u => u.UserID == id && !u.IsDeleted);
        }

        public async Task<SPM_User> AddAsync(SPM_User user)
        {
            _context.SPM_Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(SPM_User user)
        {
            _context.SPM_Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(SPM_User user)
        {
            user.IsDeleted = true;
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
