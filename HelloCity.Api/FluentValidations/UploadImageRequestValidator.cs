using FluentValidation;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.Api.FluentValidations
{
    public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageRequestValidator (ImageFileValidator validator)
        {
            RuleFor(x => x.File).NotNull().WithMessage("File is required.").SetValidator(validator);
        }
    }
}
