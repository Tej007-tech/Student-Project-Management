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
    public class SPM_TaskStatus_Controller(AppDbContext context) : ControllerBase
    {
        public readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetTaskStatus()
        {
            try
            {
                var ts = await _context.SPM_TaskStatuses.ToListAsync();
                var response = ts.Adapt<List<SPM_TaskStatusDTO>>();

                return Ok(new ApiResponse<List<SPM_TaskStatusDTO>>
                {
                    success = true,
                    message = "Task Statuses Retrieved Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<SPM_TaskStatusDTO>>
                {
                    success = false,
                    message = "An error occurred while retrieving task statuses",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskStatus(int id)
        {
            try
            {
                var ts = await _context.SPM_TaskStatuses.FindAsync(id);

                if (ts == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskStatusDTO>
                    {
                        success = false,
                        message = "Task Status Not Found",
                        data = null
                    });
                }

                return Ok(new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = true,
                    message = "Task Status Retrieved Successfully",
                    data = ts.Adapt<SPM_TaskStatusDTO>()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = false,
                    message = "An error occurred while retrieving the task status",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskStatus(SPM_TaskStatusDTO taskStDto)
        {
            try
            {
                var taskSt = taskStDto.Adapt<SPM_TaskStatus>();
                _context.SPM_TaskStatuses.Add(taskSt);
                await _context.SaveChangesAsync();

                var result = taskSt.Adapt<SPM_TaskStatusDTO>();
                return Ok(new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = true,
                    message = "Task Status Created Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = false,
                    message = "An error occurred while creating the task status",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, SPM_TaskStatusDTO taskStDto)
        {
            try
            {
                if (id != taskStDto.TaskStatusID)
                {
                    return BadRequest(new ApiResponse<SPM_TaskStatusDTO>
                    {
                        success = false,
                        message = "Task Status ID Mismatch",
                        data = null
                    });
                }

                var st = await _context.SPM_TaskStatuses.FindAsync(id);

                if (st == null)
                {
                    return NotFound(new ApiResponse<SPM_TaskStatusDTO>
                    {
                        success = false,
                        message = "Task Status Not Found",
                        data = null
                    });
                }

                taskStDto.Adapt(st);
                await _context.SaveChangesAsync();

                var result = st.Adapt<SPM_TaskStatusDTO>();
                return Ok(new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = true,
                    message = "Task Status Updated Successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<SPM_TaskStatusDTO>
                {
                    success = false,
                    message = "An error occurred while updating the task status",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskStatus(int id)
        {
            try
            {
                var st = await _context.SPM_TaskStatuses.FindAsync(id);

                if (st == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        success = false,
                        message = "Task Status Not Found",
                        data = false
                    });
                }

                _context.SPM_TaskStatuses.Remove(st);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    success = true,
                    message = "Task Status Deleted Successfully",
                    data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    success = false,
                    message = "An error occurred while deleting the task status",
                    data = false,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}
