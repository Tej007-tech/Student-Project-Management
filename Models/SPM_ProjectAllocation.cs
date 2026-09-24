using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class SPM_ProjectAllocation
    {
        [Key]
        public int ProjectAllocationID { get; set; }

        [ForeignKey("Project")]
        public int ProjectID { get; set; }

        public SPM_ProjectMaster? Project { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        public SPM_User? Student { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyID { get; set; }

        public SPM_User? Faculty { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        [Required]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        public DateTime ProjectEndDate { get; set; }

        public int TotalTasksGiven { get; set; }

        public int TotalCompletedTasks { get; set; }

        public decimal ProgressPercentage { get; set; }

        [MaxLength(5)]
        public string? OverAllGrade { get; set; }

    }
}