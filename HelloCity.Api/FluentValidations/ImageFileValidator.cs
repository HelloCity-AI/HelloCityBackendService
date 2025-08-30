using FluentValidation;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.Api.FluentValidations
{
    public class ImageFileValidator : AbstractValidator<IFormFile>
    {
        public ImageFileValidator()
        {
                RuleFor(x => x.Length).GreaterThan(0).WithMessage("Uploaded file cannot be empty");
                RuleFor(x => x.Length).LessThanOrEqualTo(5 * 1024 * 1024).WithMessage("Image file size must be less than 5MB");
                RuleFor(x => x.FileName)
                    .Must(name =>
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
                        return allowedExtensions.Contains(Path.GetExtension(name).ToLowerInvariant());
                    }).WithMessage("File type JPG, PNG, SVG is allowed only");
        }
    }
}
