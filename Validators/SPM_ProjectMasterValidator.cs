using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_ProjectMasterValidator : AbstractValidator<SPM_ProjectMasterDTO>
    {
        public SPM_ProjectMasterValidator()
        {
            RuleFor(x => x.ProjectID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Project ID must be non-negative.");

            RuleFor(x => x.ProjectTitle)
                .NotEmpty()
                .WithMessage("Project Title is required.")
                .MinimumLength(3)
                .WithMessage("Project Title must contain at least 3 characters.")
                .MaximumLength(100)
                .WithMessage("Project Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
