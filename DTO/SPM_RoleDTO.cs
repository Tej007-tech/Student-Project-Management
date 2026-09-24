using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_RoleDTO
    {
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
