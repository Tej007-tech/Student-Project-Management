using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Models
{
    public class SPM_TaskStatus
    {
        [Key]
        public int TaskStatusID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TaskStatusName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TaskStatusCssClass { get; set; } = string.Empty;

        // Navigation
        public ICollection<SPM_Task> Tasks { get; set; } = new List<SPM_Task>();
    }
}