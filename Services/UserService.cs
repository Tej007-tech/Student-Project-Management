using StudentProjectAPI.DTO;
using StudentProjectAPI.Repository;

namespace StudentProjectAPI.Services
{
    public class UserService
    {
        public interface IUserService
        {
            Task<string> AddAsync(SPM_UserDTO dto);
        }
        public class UserService : IUserService
        {
            private readonly IUserService _userRepository;

            public UserService(IUserService userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<string> AddAsync(SPM_UserDTO dto)
            {
                var user = new SPM_UserDTO
                {
                    UserTypeID = (int)dto.UserTypeID,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    
                    Email = dto.Email,
                    Password = dto.Password,
                    MobileNo = dto.MobileNo,
                     = dto.ProfilePicturePath,
                    IsActive = true,
                    IsDeleted = false
                };

                await _userRepository.AddAsync(user);

                return "Record Inserted";
            }
        }
    }
}
