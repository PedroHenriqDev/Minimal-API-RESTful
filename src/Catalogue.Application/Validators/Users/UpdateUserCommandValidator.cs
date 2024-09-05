using Catalogue.Application.Resources;
using Catalogue.Application.Settings;
using Catalogue.Application.Users.Commands.Requests;
using FluentValidation;

namespace Catalogue.Application.Validators.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommandRequest>
{
    public UpdateUserCommandValidator()
    {
        string nameMessage = string.Format(UserValidationMessagesResource.NAME_INVALID, UserSettings.MaxName);

        RuleFor(u => u.NameNew)
            .NotNull()
            .NotEmpty()
            .MaximumLength(UserSettings.MaxName)
            .WithMessage(nameMessage)
            .Custom((name, context) =>
            {
                if (!name.Any(char.IsLetter))
                {
                    context.AddFailure("Name", UserValidationMessagesResource.NAME_LETTER_INVALID);
                }
            });

        string emailMessage = string.Format(UserValidationMessagesResource.EMAIL_INVALID, UserSettings.MaxEmail);
        RuleFor(u => u.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(UserSettings.MaxEmail)
            .WithMessage(emailMessage);

        string birthDateMessage = string.Format(ErrorMessagesResource.BIRTH_DATE_INVALID,
           UserSettings.MaxDate.ToShortTimeString(),
           UserSettings.MinDate.ToShortTimeString());

        RuleFor(u => u.BirthDate)
            .NotNull()
            .GreaterThanOrEqualTo(UserSettings.MinDate)
            .LessThanOrEqualTo(UserSettings.MaxDate)
            .WithMessage(birthDateMessage);
    }
}
