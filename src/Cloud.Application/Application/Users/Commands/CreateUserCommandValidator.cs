using Cloud.Core.Application.Users.Commands;
using FluentValidation;

namespace Cloud.Application.Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // TODO: EmailExists
        RuleFor(entity => entity.Email)
            .NotEmpty()
            .WithMessage("EmailRequired")
            .EmailAddress()
            .WithMessage("EmailInvalidFormat")
            .MaximumLength(256)
            .WithMessage("EmailTooLong");

        RuleFor(entity => entity.GivenName)
            .NotEmpty()
            .WithMessage("GivenNameRequired")
            .MaximumLength(256)
            .WithMessage("GivenNameTooLong");

        RuleFor(entity => entity.FamilyName)
            .NotEmpty()
            .WithMessage("FamilyNameRequired")
            .MaximumLength(256)
            .WithMessage("FamilyNameTooLong");

        RuleFor(entity => entity.Password)
            .NotEmpty()
            .WithMessage("PasswordRequired")
            .MinimumLength(8)
            .WithMessage("PasswordMinimumLength");

        When(entity => !string.IsNullOrWhiteSpace(entity.ProfilePhotoUrl), () =>
        {
            RuleFor(entity => entity.ProfilePhotoUrl)
                .NotEmpty()
                .WithMessage("ProfilePhotoUrlRequired")
                .URL()
                .WithMessage("ProfilePhotoUrlInvalidFormat")
                .MaximumLength(2048)
                .WithMessage("ProfilePhotoUrlTooLong");
        });
    }
}
