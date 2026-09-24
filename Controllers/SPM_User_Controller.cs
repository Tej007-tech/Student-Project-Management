using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.ApiResponse;
using StudentProjectAPI.Constants;
using StudentProjectAPI.DTO;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SPM_User_Controller : ControllerBase
    {
        private readonly ISPM_UserService _userService;

        public SPM_User_Controller(ISPM_UserService userService)
        {
            _userService = userService;
        }

        // =========================
        // GET ALL USERS
        // =========================
        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                return Ok(new ApiResponse<List<SPM_UserDTO>>
                {
                    success = true,
                    message = "Users Retrieved Successfully",
                    data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_UserDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving users",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        // =========================
        // GET USER BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse<SPM_UserDTO>
                    {
                        success = false,
                        message = "User Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_UserDTO>
                {
                    success = true,
                    message = "User Retrieved Successfully",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the user",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        // =========================
        // CREATE USER
        // =========================
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateUser([FromBody] SPM_UserDTO userDto)
        {
            try
            {
                var (isValid, errors, data) = await _userService.CreateUserAsync(userDto);

                if (!isValid)
                {
                    return BadRequest(new ApiResponse<SPM_UserDTO>
                    {
                        success = false,
                        message = "Validation Failed",
                        data = null,
                        error = errors
                    });
                }

                return Ok(new ApiResponse<SPM_UserDTO>
                {
                    success = true,
                    message = "User Created Successfully",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserDTO>
                {
                    success = false,
                    message = "An error occurred while creating the user",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        // =========================
        // UPDATE USER
        // =========================
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] SPM_UserDTO userDto)
        {
            try
            {
                if (id != userDto.UserID)
                {
                    return BadRequest(new ApiResponse<SPM_UserDTO>
                    {
                        success = false,
                        message = "User ID Mismatch",
                        data = null
                    });
                }

                var (isValid, errors, data, notFound) = await _userService.UpdateUserAsync(id, userDto);

                if (!isValid)
                {
                    return BadRequest(new ApiResponse<SPM_UserDTO>
                    {
                        success = false,
                        message = "Validation Failed",
                        data = null,
                        error = errors
                    });
                }

                if (notFound)
                {
                    return NotFound(new ApiResponse<SPM_UserDTO>
                    {
                        success = false,
                        message = "User Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_UserDTO>
                {
                    success = true,
                    message = "User Updated Successfully",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserDTO>
                {
                    success = false,
                    message = "An error occurred while updating the user",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        // =========================
        // DELETE USER
        // =========================
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "User Not Found",
                        data = false
                    });
                }

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "User Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the user",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
