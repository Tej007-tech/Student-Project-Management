using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_TaskStatusValidator : AbstractValidator<SPM_TaskStatusDTO>
    {
        public SPM_TaskStatusValidator()
        {
            RuleFor(x => x.TaskStatusID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Task Status ID must be non-negative.");

            RuleFor(x => x.TaskStatusName)
                .NotEmpty()
                .WithMessage("Task Status Name is required.")
                .MinimumLength(2)
                .WithMessage("Task Status Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Task Status Name cannot exceed 50 characters.");

            RuleFor(x => x.TaskStatusCssClass)
                .NotEmpty()
                .WithMessage("Task Status CSS Class is required.")
                .MaximumLength(50)
                .WithMessage("Task Status CSS Class cannot exceed 50 characters.");
        }
    }
}
