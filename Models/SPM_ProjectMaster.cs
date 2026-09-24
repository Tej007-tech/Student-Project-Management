using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Models
{
    public class SPM_ProjectMaster
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<SPM_ProjectAllocation> ProjectAllocations { get; set; } = new List<SPM_ProjectAllocation>();
    }
}