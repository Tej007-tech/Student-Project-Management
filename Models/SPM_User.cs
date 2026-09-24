using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class SPM_User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string MobileNo { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("UserType")]
        public int UserTypeID { get; set; }

        public SPM_UserType? UserType { get; set; }


        // Navigation
        public ICollection<SPM_UserRole> UserRoles { get; set; } = new List<SPM_UserRole>();

        public ICollection<SPM_ProjectAllocation> StudentProjects { get; set; } = new List<SPM_ProjectAllocation>();

        public ICollection<SPM_ProjectAllocation> FacultyProjects { get; set; } = new List<SPM_ProjectAllocation>();
    }
}