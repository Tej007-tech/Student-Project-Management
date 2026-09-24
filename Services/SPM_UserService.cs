using FluentValidation;
using Mapster;
using StudentProjectAPI.DTO;
using StudentProjectAPI.Models;
using StudentProjectAPI.Repository;

namespace StudentProjectAPI.Services
{
    public class SPM_UserService : ISPM_UserService
    {
        private readonly ISPM_UserRepository _userRepository;
        private readonly IValidator<SPM_UserDTO> _validator;

        public SPM_UserService(ISPM_UserRepository userRepository, IValidator<SPM_UserDTO> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<List<SPM_UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Adapt<List<SPM_UserDTO>>();
        }

        public async Task<SPM_UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return user.Adapt<SPM_UserDTO>();
        }

        public async Task<(bool IsValid, List<string> Errors, SPM_UserDTO? Data)> CreateUserAsync(SPM_UserDTO userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return (false, errors, null);
            }

            var user = userDto.Adapt<SPM_User>();
            user.IsActive = true;
            user.IsDeleted = false;

            var createdUser = await _userRepository.AddAsync(user);
            var resultDto = createdUser.Adapt<SPM_UserDTO>();

            return (true, new List<string>(), resultDto);
        }

        public async Task<(bool IsValid, List<string> Errors, SPM_UserDTO? Data, bool NotFound)> UpdateUserAsync(int id, SPM_UserDTO userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return (false, errors, null, false);
            }

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return (true, new List<string>(), null, true);
            }

            userDto.Adapt(existingUser);
            await _userRepository.UpdateAsync(existingUser);

            var resultDto = existingUser.Adapt<SPM_UserDTO>();
            return (true, new List<string>(), resultDto, false);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.SoftDeleteAsync(user);
            return true;
        }
    }
}
