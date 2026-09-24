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
    [Authorize]
    public class SPM_ProjectMaster_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_ProjectMaster_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectMaster()
        {
            try
            {
                var projects = await _context.SPM_ProjectMasters.ToListAsync();
                var response = projects.Adapt<List<SPM_ProjectMasterDTO>>();

                return Ok(new ApiResponse<List<SPM_ProjectMasterDTO>>
                {
                    success = true,
                    message = "Projects Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_ProjectMasterDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving projects",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectMaster(int id)
        {
            try
            {
                var project = await _context.SPM_ProjectMasters.FindAsync(id);

                if (project == null)
                {
                    return NotFound(new ApiResponse<SPM_ProjectMasterDTO>
                    {
                        success = false,
                        message = "Project Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = true,
                    message = "Project Retrieved Successfully",
                    data = project.Adapt<SPM_ProjectMasterDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the project",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> CreateProjectMaster(SPM_ProjectMasterDTO projectDto)
        {
            try
            {
                var project = projectDto.Adapt<SPM_ProjectMaster>();
                _context.SPM_ProjectMasters.Add(project);
                await _context.SaveChangesAsync();

                var result = project.Adapt<SPM_ProjectMasterDTO>();
                return Ok(new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = true,
                    message = "Project Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = false,
                    message = "An error occurred while creating the project",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> UpdateProjectMaster(int id, SPM_ProjectMasterDTO projectDto)
        {
            try
            {
                if (id != projectDto.ProjectID)
                {
                    return BadRequest(new ApiResponse<SPM_ProjectMasterDTO>
                    {
                        success = false,
                        message = "Project ID Mismatch",
                        data = null
                    });
                }

                var oldProject = await _context.SPM_ProjectMasters.FindAsync(id);

                if (oldProject == null)
                {
                    return NotFound(new ApiResponse<SPM_ProjectMasterDTO>
                    {
                        success = false,
                        message = "Project Not Found",
                        data = null
                    });
                }

                projectDto.Adapt(oldProject);
                await _context.SaveChangesAsync();

                var result = oldProject.Adapt<SPM_ProjectMasterDTO>();
                return Ok(new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = true,
                    message = "Project Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectMasterDTO>
                {
                    success = false,
                    message = "An error occurred while updating the project",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> DeleteProjectMaster(int id)
        {
            try
            {
                var project = await _context.SPM_ProjectMasters.FindAsync(id);

                if (project == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Project Not Found",
                        data = false
                    });
                }

                _context.SPM_ProjectMasters.Remove(project);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Project Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the project",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
