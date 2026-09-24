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
    public class SPM_Role_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_Role_Controller(AppDbContext appContext)
        {
            _context = appContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                var roles = await _context.SPM_Roles.ToListAsync();
                var response = roles.Adapt<List<SPM_RoleDTO>>();

                return Ok(new ApiResponse<List<SPM_RoleDTO>>
                {
                    success = true,
                    message = "Roles Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_RoleDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving roles",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var role = await _context.SPM_Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound(new ApiResponse<SPM_RoleDTO>
                    {
                        success = false,
                        message = "Role Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_RoleDTO>
                {
                    success = true,
                    message = "Role Retrieved Successfully",
                    data = role.Adapt<SPM_RoleDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_RoleDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(SPM_RoleDTO roleDto)
        {
            try
            {
                var role = roleDto.Adapt<SPM_Role>();
                _context.SPM_Roles.Add(role);
                await _context.SaveChangesAsync();

                var result = role.Adapt<SPM_RoleDTO>();
                return Ok(new ApiResponse<SPM_RoleDTO>
                {
                    success = true,
                    message = "Role Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_RoleDTO>
                {
                    success = false,
                    message = "An error occurred while creating the role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, SPM_RoleDTO roleDto)
        {
            try
            {
                if (id != roleDto.RoleID)
                {
                    return BadRequest(new ApiResponse<SPM_RoleDTO>
                    {
                        success = false,
                        message = "Role ID Mismatch",
                        data = null
                    });
                }

                var oldRole = await _context.SPM_Roles.FindAsync(id);

                if (oldRole == null)
                {
                    return NotFound(new ApiResponse<SPM_RoleDTO>
                    {
                        success = false,
                        message = "Role Not Found",
                        data = null
                    });
                }

                roleDto.Adapt(oldRole);
                await _context.SaveChangesAsync();

                var result = oldRole.Adapt<SPM_RoleDTO>();
                return Ok(new ApiResponse<SPM_RoleDTO>
                {
                    success = true,
                    message = "Role Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_RoleDTO>
                {
                    success = false,
                    message = "An error occurred while updating the role",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _context.SPM_Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Role Not Found",
                        data = false
                    });
                }

                _context.SPM_Roles.Remove(role);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Role Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the role",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
