using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.ApiResponse;
using StudentProjectAPI.Data;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getStudentCount()
        {
            var count = await _context.SPM_Users
                .CountAsync(x => x.UserType != null && x.UserType.UserTypeName == "Student" && !x.IsDeleted);

            return Ok(new ApiResponse<int>
            {
                success = true,
                message = "Student count retrieved successfully",
                data = count
            });
        }

        [HttpGet]
        public async Task<IActionResult> getFacultyCount()
        {
            var count = await _context.SPM_Users
                .CountAsync(x => x.UserType != null && x.UserType.UserTypeName == "Faculty" && !x.IsDeleted);

            return Ok(new ApiResponse<int>
            {
                success = true,
                message = "Faculty count retrieved successfully",
                data = count
            });
        }

        [HttpGet]
        public async Task<IActionResult> getProjectCount()
        {
            var count = await _context.SPM_ProjectMasters.CountAsync();

            return Ok(new ApiResponse<int>
            {
                success = true,
                message = "Project count retrieved successfully",
                data = count
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTaskCountInEachStatus()
        {
            var data = await _context.SPM_Tasks
                .Where(x => x.TaskStatus != null)
                .GroupBy(x => x.TaskStatus!.TaskStatusName)
                .Select(t => new
                {
                    status = t.Key,
                    count = t.Count()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Task count by status retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTaskCountInEachPriority()
        {
            var data = await _context.SPM_Tasks
                .Where(x => x.TaskPriority != null)
                .GroupBy(x => x.TaskPriority!.TaskPriorityName)
                .Select(t => new
                {
                    priority = t.Key,
                    count = t.Count()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Task count by priority retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getFacultyWiseProjectCount()
        {
            var data = await _context.SPM_ProjectAllocations
                .Where(P => P.Faculty != null)
                .GroupBy(P => (P.Faculty!.FirstName + " " + P.Faculty.LastName).Trim())
                .Select(P => new
                {
                    FacultyName = P.Key,
                    ProjectCount = P.Count()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Faculty wise project count retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getStudentWiseTaskCount()
        {
            var data = await _context.SPM_Tasks
                .Where(P => P.ProjectAllocation != null && P.ProjectAllocation.Student != null)
                .GroupBy(P => (P.ProjectAllocation!.Student!.FirstName + " " + P.ProjectAllocation.Student.LastName).Trim())
                .Select(P => new
                {
                    StudentName = P.Key,
                    TaskCount = P.Count(),
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Student wise task count retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getScoreWiseTopStudent()
        {
            var data = await _context.SPM_Tasks
                .Where(P => P.ProjectAllocation != null && P.ProjectAllocation.Student != null)
                .GroupBy(P => (P.ProjectAllocation!.Student!.FirstName + " " + P.ProjectAllocation.Student.LastName).Trim())
                .Select(P => new
                {
                    StudentName = P.Key,
                    AvgScore = P.Average(p => p.EarnedScore ?? 0),
                })
                .OrderByDescending(P => P.AvgScore)
                .Take(10)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Top students by score retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getScoreWiseBottomStudent()
        {
            var data = await _context.SPM_Tasks
                .Where(P => P.ProjectAllocation != null && P.ProjectAllocation.Student != null)
                .GroupBy(P => (P.ProjectAllocation!.Student!.FirstName + " " + P.ProjectAllocation.Student.LastName).Trim())
                .Select(P => new
                {
                    StudentName = P.Key,
                    AvgScore = P.Average(p => p.EarnedScore ?? 0),
                })
                .OrderBy(P => P.AvgScore)
                .Take(10)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Bottom students by score retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getNotCompletedTask()
        {
            var now = DateTime.Now;
            var rawData = await _context.SPM_Tasks
                .Where(T => T.TaskDueDate != null && T.TaskDueDate < now && (T.TaskStatus == null || T.TaskStatus.TaskStatusName != "Completed"))
                .Select(T => new
                {
                    TaskId = T.TaskID,
                    TaskTitle = T.TaskTitle,
                    Student = T.ProjectAllocation != null && T.ProjectAllocation.Student != null ? (T.ProjectAllocation.Student.FirstName + " " + T.ProjectAllocation.Student.LastName).Trim() : string.Empty,
                    Faculty = T.ProjectAllocation != null && T.ProjectAllocation.Faculty != null ? (T.ProjectAllocation.Faculty.FirstName + " " + T.ProjectAllocation.Faculty.LastName).Trim() : string.Empty,
                    DueDate = T.TaskDueDate
                }).ToListAsync();

            var data = rawData.Select(T => new
            {
                T.TaskId,
                T.TaskTitle,
                T.Student,
                T.Faculty,
                dueDate = T.DueDate,
                DaysOverDue = (now - T.DueDate!.Value).Days
            }).ToList();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Not completed tasks retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTaskBaseOnNextfollowUpDate()
        {
            var now = DateTime.Now;
            var nextWeek = now.AddDays(7);
            var data = await _context.SPM_Tasks
                .Where(T => T.NextFollowUpDate != null && T.NextFollowUpDate > now && T.NextFollowUpDate <= nextWeek)
                .Select(T => new
                {
                    TaskId = T.TaskID,
                    TaskTitle = T.TaskTitle,
                    Student = T.ProjectAllocation != null && T.ProjectAllocation.Student != null ? (T.ProjectAllocation.Student.FirstName + " " + T.ProjectAllocation.Student.LastName).Trim() : string.Empty,
                    Faculty = T.ProjectAllocation != null && T.ProjectAllocation.Faculty != null ? (T.ProjectAllocation.Faculty.FirstName + " " + T.ProjectAllocation.Faculty.LastName).Trim() : string.Empty,
                    NextFollowDate = T.NextFollowUpDate,
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Tasks by next follow up date retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getGradeWiseStudentCount()
        {
            var data = await _context.SPM_ProjectAllocations
                .GroupBy(P => P.OverAllGrade ?? "Unassigned")
                .Select(P => new
                {
                    Grade = P.Key,
                    StudentCount = P.Count()
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Grade wise student count retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getMonthWiseCompletedTaskCount()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.TaskStatus != null && T.TaskStatus.TaskStatusName == "Completed" && T.TaskCompletedDate != null)
                .GroupBy(T => new
                {
                    Year = T.TaskCompletedDate!.Value.Year,
                    Month = T.TaskCompletedDate!.Value.Month
                })
                .Select(T => new
                {
                    Year = T.Key.Year,
                    Month = T.Key.Month,
                    TaskCount = T.Count()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Month wise completed task count retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getRoleWiseActiveUserCount()
        {
            var data = await _context.SPM_Users
                .Where(U => U.IsActive && !U.IsDeleted && U.UserType != null)
                .GroupBy(U => U.UserType!.UserTypeName)
                .Select(U => new
                {
                    Role = U.Key,
                    UserCount = U.Count()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Role wise active user count retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getRoleAndUserName()
        {
            var data = await _context.SPM_Users
                .Where(U => !U.IsDeleted)
                .Select(U => new
                {
                    Role = U.UserType != null ? U.UserType.UserTypeName : string.Empty,
                    UserName = (U.FirstName + " " + U.LastName).Trim()
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Role and user names retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getRoleWithMoreThenTenUser()
        {
            var data = await _context.SPM_Users
                .Where(U => !U.IsDeleted && U.UserType != null)
                .GroupBy(U => U.UserType!.UserTypeName)
                .Select(U => new
                {
                    Role = U.Key,
                    UserCount = U.Count()
                })
                .Where(U => U.UserCount > 10)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Roles with more than 10 users retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getRoleStatistics()
        {
            var data = await _context.SPM_Users
                .Where(U => !U.IsDeleted && U.UserType != null)
                .GroupBy(U => U.UserType!.UserTypeName)
                .Select(U => new
                {
                    Role = U.Key,
                    TotalUser = U.Count(),
                    ActiveUser = U.Count(U => U.IsActive),
                    InActiveUser = U.Count(U => !U.IsActive)
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Role statistics retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTaskDueDateWithInSevenDays()
        {
            var now = DateTime.Now;
            var nextWeek = now.AddDays(7);
            var rawData = await _context.SPM_Tasks
                .Where(T => T.TaskDueDate != null && T.TaskDueDate > now && T.TaskDueDate <= nextWeek)
                .Select(T => new
                {
                    TaskId = T.TaskID,
                    TaskTitle = T.TaskTitle,
                    Project = T.ProjectAllocation != null && T.ProjectAllocation.Project != null ? T.ProjectAllocation.Project.ProjectTitle : string.Empty,
                    Student = T.ProjectAllocation != null && T.ProjectAllocation.Student != null ? (T.ProjectAllocation.Student.FirstName + " " + T.ProjectAllocation.Student.LastName).Trim() : string.Empty,
                    DueDate = T.TaskDueDate
                }).ToListAsync();

            var data = rawData.Select(T => new
            {
                T.TaskId,
                T.TaskTitle,
                T.Project,
                T.Student,
                T.DueDate,
                RemainingDays = (T.DueDate!.Value - now).Days
            }).ToList();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Tasks due within 7 days retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTaskStatistics()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Project != null)
                .GroupBy(T => T.ProjectAllocation!.Project!.ProjectTitle)
                .Select(T => new
                {
                    Project = T.Key,
                    TotalTask = T.Count(),
                    CompletedTask = T.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Completed"),
                    PendingTask = T.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Pending"),
                    AvgProgress = T.Average(t => t.ProgressPercentage)
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Task statistics retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getProjectStatistics()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Project != null)
                .GroupBy(T => T.ProjectAllocation!.Project!.ProjectTitle)
                .Select(T => new
                {
                    Project = T.Key,
                    TotalAssignedScore = T.Sum(t => t.AssignedScore),
                    TotalEarnedScore = T.Sum(t => t.EarnedScore ?? 0),
                    ScorePercentage = T.Sum(t => t.AssignedScore) == 0
                        ? 0
                        : (double)(T.Sum(t => t.EarnedScore ?? 0) / T.Sum(t => t.AssignedScore)) * 100
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Project statistics retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTopProjectBaseOnAvgScore()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Project != null)
                .GroupBy(T => T.ProjectAllocation!.Project!.ProjectTitle)
                .Select(T => new
                {
                    Project = T.Key,
                    AvgScore = T.Average(t => t.EarnedScore ?? 0)
                })
                .OrderByDescending(T => T.AvgScore)
                .Take(10)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Top projects by average score retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getFacultyWiseProjectStatistics()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Faculty != null)
                .GroupBy(T => (T.ProjectAllocation!.Faculty!.FirstName + " " + T.ProjectAllocation.Faculty.LastName).Trim())
                .Select(T => new
                {
                    Faculty = T.Key,
                    TotalProject = T.Select(t => t.ProjectAllocation!.ProjectID).Distinct().Count(),
                    TotalTask = T.Count(),
                    AvgProgress = T.Average(t => t.ProgressPercentage)
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Faculty wise project statistics retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getStudentWiseTaskStatistics()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Student != null)
                .GroupBy(T => (T.ProjectAllocation!.Student!.FirstName + " " + T.ProjectAllocation.Student.LastName).Trim())
                .Select(T => new
                {
                    Student = T.Key,
                    TotalTask = T.Count(),
                    CompletedTask = T.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Completed"),
                    PendingTask = T.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Pending"),
                    AvgScore = T.Average(t => t.EarnedScore ?? 0)
                }).ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Student wise task statistics retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getIncompleteProject()
        {
            var now = DateTime.Now;
            var data = await _context.SPM_ProjectAllocations
                .Where(PA => PA.ProjectEndDate < now && PA.ProgressPercentage < 100)
                .Select(PA => new
                {
                    Project = PA.Project != null ? PA.Project.ProjectTitle : string.Empty,
                    Student = PA.Student != null ? (PA.Student.FirstName + " " + PA.Student.LastName).Trim() : string.Empty,
                    Faculty = PA.Faculty != null ? (PA.Faculty.FirstName + " " + PA.Faculty.LastName).Trim() : string.Empty,
                    EndDate = PA.ProjectEndDate,
                    Progress = PA.ProgressPercentage
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Incomplete projects retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getMonthWiseCompletedTask()
        {
            var data = await _context.SPM_Tasks
                .Where(T => T.TaskStatus != null && T.TaskStatus.TaskStatusName == "Completed" && T.TaskCompletedDate != null)
                .GroupBy(T => new
                {
                    Year = T.TaskCompletedDate!.Value.Year,
                    Month = T.TaskCompletedDate!.Value.Month
                })
                .Select(T => new
                {
                    Year = T.Key.Year,
                    Month = T.Key.Month,
                    CompletedTasks = T.Count()
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Month wise completed tasks retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getTopFacultyBaseOnProjectProgress()
        {
            var data = await _context.SPM_ProjectAllocations
                .Where(PA => PA.Faculty != null)
                .GroupBy(PA => (PA.Faculty!.FirstName + " " + PA.Faculty.LastName).Trim())
                .Select(G => new
                {
                    Faculty = G.Key,
                    AvgProgress = G.Average(PA => PA.ProgressPercentage)
                })
                .OrderByDescending(x => x.AvgProgress)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Top faculty by project progress retrieved successfully",
                data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> getProjectWiseTaskStatistics()
        {
            var now = DateTime.Now;
            var data = await _context.SPM_Tasks
                .Where(T => T.ProjectAllocation != null && T.ProjectAllocation.Project != null)
                .GroupBy(T => T.ProjectAllocation!.Project!.ProjectTitle)
                .Select(G => new
                {
                    Project = G.Key,
                    TotalTasks = G.Count(),
                    Completed = G.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Completed"),
                    Pending = G.Count(t => t.TaskStatus != null && t.TaskStatus.TaskStatusName == "Pending"),
                    Overdue = G.Count(t => t.TaskDueDate != null && t.TaskDueDate < now && (t.TaskStatus == null || t.TaskStatus.TaskStatusName != "Completed"))
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                success = true,
                message = "Project wise task statistics retrieved successfully",
                data = data
            });
        }
    }
}
