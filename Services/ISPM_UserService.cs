using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Services
{
    public interface ISPM_UserService
    {
        Task<List<SPM_UserDTO>> GetAllUsersAsync();
        Task<SPM_UserDTO?> GetUserByIdAsync(int id);
        Task<(bool IsValid, List<string> Errors, SPM_UserDTO? Data)> CreateUserAsync(SPM_UserDTO userDto);
        Task<(bool IsValid, List<string> Errors, SPM_UserDTO? Data, bool NotFound)> UpdateUserAsync(int id, SPM_UserDTO userDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
