using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_RoleValidator : AbstractValidator<SPM_RoleDTO>
    {
        public SPM_RoleValidator()
        {
            RuleFor(x => x.RoleID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Role ID must be non-negative.");

            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("Role Name is required.")
                .MinimumLength(2)
                .WithMessage("Role Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Role Name cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithMessage("Description cannot exceed 250 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
