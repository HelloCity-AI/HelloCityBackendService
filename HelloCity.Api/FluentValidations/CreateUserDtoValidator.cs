using FluentValidation;
using HelloCity.Api.DTOs.Users;
using HelloCity.Models.Enums;
namespace HelloCity.FluentValidations
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must be at most 50 characters.")
                .MinimumLength(1).WithMessage("Username must be at least 1 characters. ");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(100).WithMessage("Password must be at most 100 characters.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender is invalid.");

            RuleFor(x => x.PreferredLanguage)
                .IsInEnum().WithMessage("Preferred language is invalid.");

            RuleFor(x => x.Nationality)
                .MaximumLength(100).WithMessage("Nationality must be at most 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Nationality));

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City must be at most 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.City));
        }

    }
}
