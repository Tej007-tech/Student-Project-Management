using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_TaskStatusDTO
    {
        public int TaskStatusID { get; set; }

        [Required]
        public string TaskStatusName { get; set; } = string.Empty;

        [Required]
        public string TaskStatusCssClass { get; set; } = string.Empty;
    }
}
