using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class SPM_Task
    {
        [Key]
        public int TaskID { get; set; }

        [ForeignKey("ProjectAllocation")]
        public int ProjectAllocationID { get; set; }

        public SPM_ProjectAllocation? ProjectAllocation { get; set; }

        [Required]
        [MaxLength(200)]
        public string TaskTitle { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        [ForeignKey("TaskStatus")]
        public int TaskStatusID { get; set; }

        public SPM_TaskStatus? TaskStatus { get; set; }

        [ForeignKey("TaskPriority")]
        public int TaskPriorityID { get; set; }

        public SPM_TaskPriority? TaskPriority { get; set; }

        public decimal AssignedScore { get; set; }

        public decimal? EarnedScore { get; set; }

        public decimal ProgressPercentage { get; set; }

        public DateTime TaskAssignedDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskDueDate { get; set; }

        public DateTime? TaskCompletedDate { get; set; }

        public DateTime? NextFollowUpDate { get; set; }

        [MaxLength(500)]
        public string? FacultyRemarks { get; set; }

        [MaxLength(500)]
        public string? StudentRemarks { get; set; }
    }
}