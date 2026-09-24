using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_TaskValidator : AbstractValidator<SPM_TaskDTO>
    {
        public SPM_TaskValidator()
        {
            RuleFor(x => x.TaskID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Task ID must be non-negative.");

            RuleFor(x => x.ProjectAllocationID)
                .GreaterThan(0)
                .WithMessage("Project Allocation ID must be greater than 0.");

            RuleFor(x => x.TaskTitle)
                .NotEmpty()
                .WithMessage("Task Title is required.")
                .MinimumLength(3)
                .WithMessage("Task Title must contain at least 3 characters.")
                .MaximumLength(150)
                .WithMessage("Task Title cannot exceed 150 characters.");

            RuleFor(x => x.TaskDescription)
                .MaximumLength(1000)
                .WithMessage("Task Description cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaskDescription));

            RuleFor(x => x.TaskStatusID)
                .GreaterThan(0)
                .WithMessage("Task Status ID must be greater than 0.");

            RuleFor(x => x.TaskPriorityID)
                .GreaterThan(0)
                .WithMessage("Task Priority ID must be greater than 0.");

            RuleFor(x => x.AssignedScore)
                .InclusiveBetween(0, 100)
                .WithMessage("Assigned Score must be between 0 and 100.");

            RuleFor(x => x.EarnedScore)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Earned Score must be non-negative.")
                .LessThanOrEqualTo(x => x.AssignedScore)
                .WithMessage("Earned Score cannot exceed Assigned Score.")
                .When(x => x.EarnedScore.HasValue);

            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Progress Percentage must be between 0 and 100.");

            RuleFor(x => x.TaskAssignedDate)
                .NotEmpty()
                .WithMessage("Task Assigned Date is required.");

            RuleFor(x => x.TaskStartDate)
                .GreaterThanOrEqualTo(x => x.TaskAssignedDate)
                .WithMessage("Task Start Date cannot be earlier than Task Assigned Date.")
                .When(x => x.TaskStartDate.HasValue);

            RuleFor(x => x.TaskDueDate)
                .GreaterThan(x => x.TaskAssignedDate)
                .WithMessage("Task Due Date must be after Task Assigned Date.")
                .When(x => x.TaskDueDate.HasValue);

            RuleFor(x => x.TaskCompletedDate)
                .GreaterThanOrEqualTo(x => x.TaskAssignedDate)
                .WithMessage("Task Completed Date cannot be earlier than Task Assigned Date.")
                .When(x => x.TaskCompletedDate.HasValue);

            RuleFor(x => x.FacultyRemarks)
                .MaximumLength(500)
                .WithMessage("Faculty Remarks cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.FacultyRemarks));

            RuleFor(x => x.StudentRemarks)
                .MaximumLength(500)
                .WithMessage("Student Remarks cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.StudentRemarks));
        }
    }
}
