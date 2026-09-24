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
    public class SPM_Task_Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public SPM_Task_Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTask()
        {
            try
            {
                var tasks = await _context.SPM_Tasks.ToListAsync();
                var response = tasks.Adapt<List<SPM_TaskDTO>>();

                return Ok(new ApiResponse<List<SPM_TaskDTO>>
                {
                    success = true,
                    message = "Tasks Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_TaskDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving tasks",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _context.SPM_Tasks.FindAsync(id);

                if (task == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskDTO>
                    {
                        success = false,
                        message = "Task Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_TaskDTO>
                {
                    success = true,
                    message = "Task Retrieved Successfully",
                    data = task.Adapt<SPM_TaskDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the task",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(SPM_TaskDTO taskDto)
        {
            try
            {
                var task = taskDto.Adapt<SPM_Task>();
                _context.SPM_Tasks.Add(task);
                await _context.SaveChangesAsync();

                var result = task.Adapt<SPM_TaskDTO>();
                return Ok(new ApiResponse<SPM_TaskDTO>
                {
                    success = true,
                    message = "Task Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskDTO>
                {
                    success = false,
                    message = "An error occurred while creating the task",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, SPM_TaskDTO taskDto)
        {
            try
            {
                if (id != taskDto.TaskID)
                {
                    return BadRequest(new ApiResponse<SPM_TaskDTO>
                    {
                        success = false,
                        message = "Task ID Mismatch",
                        data = null
                    });
                }

                var oldTask = await _context.SPM_Tasks.FindAsync(id);

                if (oldTask == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskDTO>
                    {
                        success = false,
                        message = "Task Not Found",
                        data = null
                    });
                }

                taskDto.Adapt(oldTask);
                await _context.SaveChangesAsync();

                var result = oldTask.Adapt<SPM_TaskDTO>();
                return Ok(new ApiResponse<SPM_TaskDTO>
                {
                    success = true,
                    message = "Task Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskDTO>
                {
                    success = false,
                    message = "An error occurred while updating the task",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.SPM_Tasks.FindAsync(id);

                if (task == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Task Not Found",
                        data = false
                    });
                }

                _context.SPM_Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Task Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the task",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
