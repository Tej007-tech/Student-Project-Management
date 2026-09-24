using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Models
{
    public class SPM_Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation
        
        public ICollection<SPM_UserRole> UserRoles { get; set; } = new List<SPM_UserRole>();
    }
}