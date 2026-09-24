using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_ProjectMasterDTO
    {
        public int ProjectID { get; set; }

        [Required]
        public string ProjectTitle { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
