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
    public class SPM_ProjectAllocation_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_ProjectAllocation_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectAllocation()
        {
            try
            {
                var allocations = await _context.SPM_ProjectAllocations.ToListAsync();
                var response = allocations.Adapt<List<SPM_ProjectAllocationDTO>>();

                return Ok(new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = true,
                    message = "Project Allocations Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving project allocations",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetProjectAllocationsByStudent(int studentId)
        {
            try
            {
                var allocations = await _context.SPM_ProjectAllocations
                    .Where(a => a.StudentID == studentId)
                    .ToListAsync();

                var response = allocations.Adapt<List<SPM_ProjectAllocationDTO>>();

                return Ok(new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = true,
                    message = "Student Project Allocations Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving student project allocations",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<IActionResult> GetProjectAllocationsByFaculty(int facultyId)
        {
            try
            {
                var allocations = await _context.SPM_ProjectAllocations
                    .Where(a => a.FacultyID == facultyId)
                    .ToListAsync();

                var response = allocations.Adapt<List<SPM_ProjectAllocationDTO>>();

                return Ok(new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = true,
                    message = "Faculty Project Allocations Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_ProjectAllocationDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving faculty project allocations",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectAllocation(int id)
        {
            try
            {
                var allocation = await _context.SPM_ProjectAllocations.FindAsync(id);

                if (allocation == null)
                {
                    return NotFound(new ApiResponse<SPM_ProjectAllocationDTO>
                    {
                        success = false,
                        message = "Project Allocation Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = true,
                    message = "Project Allocation Retrieved Successfully",
                    data = allocation.Adapt<SPM_ProjectAllocationDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the project allocation",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> CreateProjectAllocation(SPM_ProjectAllocationDTO allocationDto)
        {
            try
            {
                var allocation = allocationDto.Adapt<SPM_ProjectAllocation>();
                _context.SPM_ProjectAllocations.Add(allocation);
                await _context.SaveChangesAsync();

                var result = allocation.Adapt<SPM_ProjectAllocationDTO>();
                return Ok(new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = true,
                    message = "Project Allocation Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = false,
                    message = "An error occurred while creating the project allocation",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> UpdateProjectAllocation(int id, SPM_ProjectAllocationDTO allocationDto)
        {
            try
            {
                if (id != allocationDto.ProjectAllocationID)
                {
                    return BadRequest(new ApiResponse<SPM_ProjectAllocationDTO>
                    {
                        success = false,
                        message = "Project Allocation ID Mismatch",
                        data = null
                    });
                }

                var oldAllocation = await _context.SPM_ProjectAllocations.FindAsync(id);

                if (oldAllocation == null)
                {
                    return NotFound(new ApiResponse<SPM_ProjectAllocationDTO>
                    {
                        success = false,
                        message = "Project Allocation Not Found",
                        data = null
                    });
                }

                allocationDto.Adapt(oldAllocation);
                await _context.SaveChangesAsync();

                var result = oldAllocation.Adapt<SPM_ProjectAllocationDTO>();
                return Ok(new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = true,
                    message = "Project Allocation Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_ProjectAllocationDTO>
                {
                    success = false,
                    message = "An error occurred while updating the project allocation",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> DeleteProjectAllocation(int id)
        {
            try
            {
                var allocation = await _context.SPM_ProjectAllocations.FindAsync(id);

                if (allocation == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Project Allocation Not Found",
                        data = false
                    });
                }

                _context.SPM_ProjectAllocations.Remove(allocation);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Project Allocation Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the project allocation",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
