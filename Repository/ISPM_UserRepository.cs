using StudentProjectAPI.Models;

namespace StudentProjectAPI.Repository
{
    public interface ISPM_UserRepository
    {
        Task<List<SPM_User>> GetAllAsync();
        Task<SPM_User?> GetByIdAsync(int id);
        Task<SPM_User> AddAsync(SPM_User user);
        Task UpdateAsync(SPM_User user);
        Task SoftDeleteAsync(SPM_User user);
    }
}
