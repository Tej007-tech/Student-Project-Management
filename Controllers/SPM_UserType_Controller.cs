using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StudentProjectAPI.ApiResponse;
using StudentProjectAPI.Constants;
using StudentProjectAPI.Data;
using StudentProjectAPI.DTO;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class SPM_UserType_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_UserType_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserType()
        {
            try
            {
                var userTypes = await _context.SPM_UserTypes.ToListAsync();
                var response = userTypes.Adapt<List<SPM_UserTypeDTO>>();

                return Ok(new ApiResponse<List<SPM_UserTypeDTO>>
                {
                    success = true,
                    message = "User Types Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_UserTypeDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving user types",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserType(int id)
        {
            try
            {
                var userType = await _context.SPM_UserTypes.FindAsync(id);

                if (userType == null)
                {
                    return NotFound(new ApiResponse<SPM_UserTypeDTO>
                    {
                        success = false,
                        message = "User Type Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_UserTypeDTO>
                {
                    success = true,
                    message = "User Type Retrieved Successfully",
                    data = userType.Adapt<SPM_UserTypeDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserTypeDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the user type",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserType(SPM_UserTypeDTO userTypeDto)
        {
            try
            {
                var userType = userTypeDto.Adapt<SPM_UserType>();
                _context.SPM_UserTypes.Add(userType);
                await _context.SaveChangesAsync();

                var result = userType.Adapt<SPM_UserTypeDTO>();
                return Ok(new ApiResponse<SPM_UserTypeDTO>
                {
                    success = true,
                    message = "User Type Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserTypeDTO>
                {
                    success = false,
                    message = "An error occurred while creating the user type",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserType(int id, SPM_UserTypeDTO userTypeDto)
        {
            try
            {
                if (id != userTypeDto.UserTypeID)
                {
                    return BadRequest(new ApiResponse<SPM_UserTypeDTO>
                    {
                        success = false,
                        message = "User Type ID Mismatch",
                        data = null
                    });
                }

                var oldUserType = await _context.SPM_UserTypes.FindAsync(id);

                if (oldUserType == null)
                {
                    return NotFound(new ApiResponse<SPM_UserTypeDTO>
                    {
                        success = false,
                        message = "User Type Not Found",
                        data = null
                    });
                }

                userTypeDto.Adapt(oldUserType);
                await _context.SaveChangesAsync();

                var result = oldUserType.Adapt<SPM_UserTypeDTO>();
                return Ok(new ApiResponse<SPM_UserTypeDTO>
                {
                    success = true,
                    message = "User Type Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserTypeDTO>
                {
                    success = false,
                    message = "An error occurred while updating the user type",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            try
            {
                var userType = await _context.SPM_UserTypes.FindAsync(id);

                if (userType == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "User Type Not Found",
                        data = false
                    });
                }

                _context.SPM_UserTypes.Remove(userType);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "User Type Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the user type",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
