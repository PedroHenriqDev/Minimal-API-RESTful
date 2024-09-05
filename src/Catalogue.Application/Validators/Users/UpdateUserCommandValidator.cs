using Catalogue.Application.Resources;
using Catalogue.Application.Settings;
using Catalogue.Application.Users.Commands.Requests;
using FluentValidation;

namespace Catalogue.Application.Validators.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommandRequest>
{
    public UpdateUserCommandValidator()
    {
        const int MAX_NAME_LENGTH = 255;
        string nameMessage = string.Format(UserValidationMessagesResource.NAME_INVALID, MAX_NAME_LENGTH);

        RuleFor(u => u.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MAX_NAME_LENGTH)
            .WithMessage(nameMessage)
            .Custom((name, context) =>
            {
                if (!name.Any(char.IsLetter))
                {
                    context.AddFailure("Name", UserValidationMessagesResource.NAME_LETTER_INVALID);
                }
            });

        const int MAX_EMAIL_LENGTH = 256;
        string emailMessage = string.Format(UserValidationMessagesResource.EMAIL_INVALID, MAX_EMAIL_LENGTH);
        RuleFor(u => u.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(MAX_EMAIL_LENGTH)
            .WithMessage(emailMessage);

        string birthDateMessage = string.Format(ErrorMessagesResource.BIRTH_DATE_INVALID,
            DateSettings.MaxDate.ToShortTimeString(),
            DateSettings.MinDate.ToShortTimeString());

        RuleFor(u => u.BirthDate)
            .NotNull()
            .GreaterThanOrEqualTo(DateSettings.MinDate)
            .LessThanOrEqualTo(DateSettings.MaxDate)
            .WithMessage(birthDateMessage);
    }
}
