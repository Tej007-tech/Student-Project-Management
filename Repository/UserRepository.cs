using StudentProjectAPI.Data;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Repository
{
  
    public class UserRepository : IUserRepository
    {
        private readonly AppContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SPM_UserDTO user)
        {
            await _context.SPM_Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
