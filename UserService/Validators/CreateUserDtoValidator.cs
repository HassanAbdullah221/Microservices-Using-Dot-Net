using FluentValidation;
using WebApplication4.DTOs.User;

namespace WebApplication4.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
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
                .WithMessage("Phone number must contain exactly 10 digits");

            RuleFor(x => x.Password)
     .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
     .WithMessage("Password must be at least 8 characters and include uppercase, lowercase, number, and special character");

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
