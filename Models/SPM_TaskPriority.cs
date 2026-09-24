using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Models
{
    public class SPM_TaskPriority
    {
        [Key]
        public int TaskPriorityID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TaskPriorityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TaskPriorityCssClass { get; set; } = string.Empty;

        // Navigation
        public ICollection<SPM_Task> Tasks { get; set; } = new List<SPM_Task>();
    }
}