using FluentValidation;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_UserRoleValidator : AbstractValidator<SPM_UserRoleDTO>
    {
        public SPM_UserRoleValidator()
        {
            RuleFor(x => x.RolePermissionID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Role Permission ID must be non-negative.");

            RuleFor(x => x.RoleID)
                .GreaterThan(0)
                .WithMessage("Role ID must be greater than 0.");

            RuleFor(x => x.UserID)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0.");
        }
    }
}
