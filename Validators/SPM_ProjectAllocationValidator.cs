using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_ProjectAllocationValidator : AbstractValidator<SPM_ProjectAllocationDTO>
    {
        public SPM_ProjectAllocationValidator()
        {
            RuleFor(x => x.ProjectAllocationID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Project Allocation ID must be non-negative.");

            RuleFor(x => x.ProjectID)
                .GreaterThan(0)
                .WithMessage("Project ID must be greater than 0.");

            RuleFor(x => x.StudentID)
                .GreaterThan(0)
                .WithMessage("Student ID must be greater than 0.");

            RuleFor(x => x.FacultyID)
                .GreaterThan(0)
                .WithMessage("Faculty ID must be greater than 0.");

            RuleFor(x => x.AssignedDate)
                .NotEmpty()
                .WithMessage("Assigned Date is required.");

            RuleFor(x => x.ProjectStartDate)
                .NotEmpty()
                .WithMessage("Project Start Date is required.")
                .GreaterThanOrEqualTo(x => x.AssignedDate)
                .WithMessage("Project Start Date cannot be earlier than Assigned Date.");

            RuleFor(x => x.ProjectEndDate)
                .NotEmpty()
                .WithMessage("Project End Date is required.")
                .GreaterThan(x => x.ProjectStartDate)
                .WithMessage("Project End Date must be after Project Start Date.");

            RuleFor(x => x.TotalTasksGiven)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total Tasks Given must be non-negative.");

            RuleFor(x => x.TotalCompletedTasks)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total Completed Tasks must be non-negative.")
                .LessThanOrEqualTo(x => x.TotalTasksGiven)
                .WithMessage("Total Completed Tasks cannot exceed Total Tasks Given.");

            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Progress Percentage must be between 0 and 100.");

            RuleFor(x => x.OverAllGrade)
                .MaximumLength(10)
                .WithMessage("Overall Grade cannot exceed 10 characters.")
                .When(x => !string.IsNullOrEmpty(x.OverAllGrade));
        }
    }
}
