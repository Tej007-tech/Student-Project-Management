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
    public class SPM_UserRole_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_UserRole_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRole()
        {
            try
            {
                var userRoles = await _context.SPM_UserRoles.ToListAsync();
                var response = userRoles.Adapt<List<SPM_UserRoleDTO>>();

                return Ok(new ApiResponse<List<SPM_UserRoleDTO>>
                {
                    success = true,
                    message = "User Roles Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_UserRoleDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving user roles",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRole(int id)
        {
            try
            {
                var userRole = await _context.SPM_UserRoles.FindAsync(id);

                if (userRole == null)
                {
                    return NotFound(new ApiResponse<SPM_UserRoleDTO>
                    {
                        success = false,
                        message = "User Role Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_UserRoleDTO>
                {
                    success = true,
                    message = "User Role Retrieved Successfully",
                    data = userRole.Adapt<SPM_UserRoleDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserRoleDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the user role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole(SPM_UserRoleDTO userRoleDto)
        {
            try
            {
                var userRole = userRoleDto.Adapt<SPM_UserRole>();
                _context.SPM_UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                var result = userRole.Adapt<SPM_UserRoleDTO>();
                return Ok(new ApiResponse<SPM_UserRoleDTO>
                {
                    success = true,
                    message = "User Role Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserRoleDTO>
                {
                    success = false,
                    message = "An error occurred while creating the user role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRole(int id, SPM_UserRoleDTO userRoleDto)
        {
            try
            {
                if (id != userRoleDto.RolePermissionID)
                {
                    return BadRequest(new ApiResponse<SPM_UserRoleDTO>
                    {
                        success = false,
                        message = "Role Permission ID Mismatch",
                        data = null
                    });
                }

                var oldUserRole = await _context.SPM_UserRoles.FindAsync(id);

                if (oldUserRole == null)
                {
                    return NotFound(new ApiResponse<SPM_UserRoleDTO>
                    {
                        success = false,
                        message = "User Role Not Found",
                        data = null
                    });
                }

                userRoleDto.Adapt(oldUserRole);
                await _context.SaveChangesAsync();

                var result = oldUserRole.Adapt<SPM_UserRoleDTO>();
                return Ok(new ApiResponse<SPM_UserRoleDTO>
                {
                    success = true,
                    message = "User Role Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_UserRoleDTO>
                {
                    success = false,
                    message = "An error occurred while updating the user role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            try
            {
                var userRole = await _context.SPM_UserRoles.FindAsync(id);

                if (userRole == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "User Role Not Found",
                        data = false
                    });
                }

                _context.SPM_UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "User Role Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the user role",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
