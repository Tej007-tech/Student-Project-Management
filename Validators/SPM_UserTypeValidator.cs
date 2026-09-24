using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_UserTypeValidator : AbstractValidator<SPM_UserTypeDTO>
    {
        public SPM_UserTypeValidator()
        {
            RuleFor(x => x.UserTypeID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("User Type ID must be non-negative.");

            RuleFor(x => x.UserTypeName)
                .NotEmpty()
                .WithMessage("User Type Name is required.")
                .MinimumLength(2)
                .WithMessage("User Type Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("User Type Name cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithMessage("Description cannot exceed 250 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
