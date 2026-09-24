using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.DTO;

namespace StudentProjectAPI.Validators
{
    public class SPM_UserValidator : AbstractValidator<SPM_UserDTO>
    {
        private readonly AppDbContext _context;

        public SPM_UserValidator(AppDbContext context)
        {
            _context = context;

            // UserID
            RuleFor(x => x.UserID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("User ID must be non-negative.");

            // UserTypeID
            RuleFor(x => x.UserTypeID)
                .GreaterThan(0)
                .WithMessage("User Type ID must be greater than 0.");

            // FirstName
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .MinimumLength(2)
                .WithMessage("First Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("First Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("First Name can only contain letters and spaces.");

            // LastName
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .MinimumLength(2)
                .WithMessage("Last Name must contain at least 2 characters.")
                .MaximumLength(50)
                .WithMessage("Last Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Last Name can only contain letters and spaces.");

            // Email
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.")
                .MaximumLength(100)
                .WithMessage("Email cannot exceed 100 characters.");

            // Password
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
                .MaximumLength(50)
                .WithMessage("Password cannot exceed 50 characters.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");

            // MobileNo
            RuleFor(x => x.MobileNo)
                .NotEmpty()
                .WithMessage("Mobile Number is required.")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Mobile Number must contain exactly 10 digits.")
                .MustAsync(async (user, mobileNo, cancellation) =>
                {
                    return !await _context.SPM_Users
                        .AnyAsync(x => x.MobileNo == mobileNo && x.UserID != user.UserID && !x.IsDeleted, cancellation);
                }).WithMessage("Mobile Number already exists.");

            // IsActive
            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage("IsActive status is required.");

            // IsDeleted
            RuleFor(x => x.IsDeleted)
                .NotNull()
                .WithMessage("IsDeleted status is required.");
        }
    }
}
