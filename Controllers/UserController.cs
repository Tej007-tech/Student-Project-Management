using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.ApiResponse;
using StudentProjectAPI.Constants;
using StudentProjectAPI.Data;
using StudentProjectAPI.DTO;
using StudentProjectAPI.Models;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public UserController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var user = await _context.SPM_Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserType)
                    .SingleOrDefaultAsync(u =>
                        u.Email == dto.Email &&
                        u.Password == dto.Password &&
                        !u.IsDeleted);

                if (user == null)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        success = false,
                        message = "Invalid Email or password",
                        data = null
                    });
                }

                var token = _tokenService.GenerateToken(user);
                var roles = user.UserRoles
                    .Where(ur => ur.Role != null && !string.IsNullOrWhiteSpace(ur.Role.RoleName))
                    .Select(ur => ur.Role!.RoleName)
                    .Distinct()
                    .ToList();

                if (user.UserType != null && !string.IsNullOrWhiteSpace(user.UserType.UserTypeName) && !roles.Contains(user.UserType.UserTypeName))
                {
                    roles.Add(user.UserType.UserTypeName);
                }

                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Login successful",
                    data = new
                    {
                        Token = token,
                        User = new
                        {
                            user.UserID,
                            user.Email,
                            user.FirstName,
                            user.LastName,
                            user.UserTypeID,
                            UserTypeName = user.UserType?.UserTypeName,
                            Roles = roles
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    success = false,
                    message = "An error occurred during login",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Faculty}")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _context.SPM_Users
                    .Where(u => !u.IsDeleted)
                    .Select(u => new SPM_UserDTO
                    {
                        UserID = u.UserID,
                        UserTypeID = u.UserTypeID,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Password = u.Password,
                        MobileNo = u.MobileNo,
                        IsActive = u.IsActive,
                        IsDeleted = u.IsDeleted
                    })
                    .ToListAsync();

                return Ok(new ApiResponse<List<SPM_UserDTO>>
                {
                    success = true,
                    message = "Users retrieved successfully",
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("students-by-category")]
        public async Task<IActionResult> GetAllStudentsByCategory()
        {
            try
            {
                // Retrieve active users with UserType navigation property
                var users = await _context.SPM_Users
                    .Include(u => u.UserType)
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();

                // Group in-memory to prevent EF Core LINQ translation errors on nested DTO projections
                var usersByCategory = users
                    .GroupBy(u => u.UserType != null && !string.IsNullOrWhiteSpace(u.UserType.UserTypeName)
                        ? u.UserType.UserTypeName
                        : "Unassigned")
                    .Select(g => new
                    {
                        Category = g.Key,
                        Count = g.Count(),
                        Users = g.Select(u => new SPM_UserDTO
                        {
                            UserID = u.UserID,
                            UserTypeID = u.UserTypeID,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            MobileNo = u.MobileNo,
                            IsActive = u.IsActive,
                            IsDeleted = u.IsDeleted
                        }).ToList()
                    })
                    .ToList();

                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Student data by category retrieved successfully",
                    data = usersByCategory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    success = false,
                    message = "An error occurred while retrieving data",
                    data = null,
                    error = new List<string> { ex.Message }
                });
            }
        }
    }
}

