using FluentValidation;
using WebApplication4.DTOs.User;

namespace WebApplication4.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(150)
                .Matches(@"^[\p{L}]+(?:\s[\p{L}]+)*$")
                .WithMessage("Full name must contain letters only");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(30);

            RuleFor(x => x.Department)
                .NotEmpty()
                .MaximumLength(20)
                .Matches(@"^[\p{L}\s]+$")
                .WithMessage("Department must contain letters only");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\d{10}$")
                .WithMessage("Phone number must contain 10 digits");

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20)
                .Must(role => role == "Admin" || role == "User" || role == "Employee")
                .WithMessage("Invalid role selected");
        }
    }
}
