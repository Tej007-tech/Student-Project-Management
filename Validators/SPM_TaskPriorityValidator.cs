using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_TaskPriorityValidator : AbstractValidator<SPM_TaskPriorityDTO>
    {
        public SPM_TaskPriorityValidator()
        {
            RuleFor(x => x.TaskPriorityID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Task Priority ID must be non-negative.");

            RuleFor(x => x.TaskPriorityName)
                .NotEmpty()
                .WithMessage("Task Priority Name is required.")
                .MinimumLength(2)
                .WithMessage("Task Priority Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Task Priority Name cannot exceed 50 characters.");

            RuleFor(x => x.TaskPriorityCssClass)
                .NotEmpty()
                .WithMessage("Task Priority CSS Class is required.")
                .MaximumLength(50)
                .WithMessage("Task Priority CSS Class cannot exceed 50 characters.");
        }
    }
}
