using FluentValidation;
using WebApplication4.DTOs.Auth;

namespace WebApplication4.Validators
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format")
                .MaximumLength(30)
                .WithMessage("Email must not exceed 30 characters");
        }
    }
}
