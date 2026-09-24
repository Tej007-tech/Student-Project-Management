using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_TaskPriorityDTO
    {
        public int TaskPriorityID { get; set; }

        [Required]
        public string TaskPriorityName { get; set; } = string.Empty;

        [Required]
        public string TaskPriorityCssClass { get; set; } = string.Empty;
    }
}
