using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Models
{
    public class SPM_UserType
    {
        [Key]
        public int UserTypeID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserTypeName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<SPM_User> Users { get; set; } = new List<SPM_User>();
    }
}