using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using StudentProjectAPI.ApiResponse;
using StudentProjectAPI.Data;
using StudentProjectAPI.DTO;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SPM_TaskPriority_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_TaskPriority_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskPriority()
        {
            try
            {
                var priorities = await _context.SPM_TaskPriorities.ToListAsync();
                var response = priorities.Adapt<List<SPM_TaskPriorityDTO>>();

                return Ok(new ApiResponse<List<SPM_TaskPriorityDTO>>
                {
                    success = true,
                    message = "Task Priorities Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_TaskPriorityDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving task priorities",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskPriority(int id)
        {
            try
            {
                var priority = await _context.SPM_TaskPriorities.FindAsync(id);

                if (priority == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskPriorityDTO>
                    {
                        success = false,
                        message = "Task Priority Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = true,
                    message = "Task Priority Retrieved Successfully",
                    data = priority.Adapt<SPM_TaskPriorityDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the task priority",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskPriority(SPM_TaskPriorityDTO priorityDto)
        {
            try
            {
                var priority = priorityDto.Adapt<SPM_TaskPriority>();
                _context.SPM_TaskPriorities.Add(priority);
                await _context.SaveChangesAsync();

                var result = priority.Adapt<SPM_TaskPriorityDTO>();
                return Ok(new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = true,
                    message = "Task Priority Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = false,
                    message = "An error occurred while creating the task priority",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskPriority(int id, SPM_TaskPriorityDTO priorityDto)
        {
            try
            {
                if (id != priorityDto.TaskPriorityID)
                {
                    return BadRequest(new ApiResponse<SPM_TaskPriorityDTO>
                    {
                        success = false,
                        message = "Task Priority ID Mismatch",
                        data = null
                    });
                }

                var oldPriority = await _context.SPM_TaskPriorities.FindAsync(id);

                if (oldPriority == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskPriorityDTO>
                    {
                        success = false,
                        message = "Task Priority Not Found",
                        data = null
                    });
                }

                priorityDto.Adapt(oldPriority);
                await _context.SaveChangesAsync();

                var result = oldPriority.Adapt<SPM_TaskPriorityDTO>();
                return Ok(new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = true,
                    message = "Task Priority Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskPriorityDTO>
                {
                    success = false,
                    message = "An error occurred while updating the task priority",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskPriority(int id)
        {
            try
            {
                var priority = await _context.SPM_TaskPriorities.FindAsync(id);

                if (priority == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Task Priority Not Found",
                        data = false
                    });
                }

                _context.SPM_TaskPriorities.Remove(priority);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Task Priority Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the task priority",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
