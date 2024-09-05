using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Resources;
using FluentValidation;
using Catalogue.Application.Settings;

namespace Catalogue.Application.Validators.Users;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandValidator()
    {
        string nameMessage = string.Format(UserValidationMessagesResource.NAME_INVALID, UserSettings.MaxName);

        RuleFor(u => u.Name)
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

        string passwordMessage = string.Format(UserValidationMessagesResource.PASSWORD_INVALID, UserSettings.MaxPassword);
        RuleFor(u => u.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(UserSettings.MaxPassword)
            .WithMessage(passwordMessage)
            .Custom((password, context) => 
            {
                if(!password.Any(char.IsLetter) || !password.Any(char.IsNumber)) 
                {
                    context.AddFailure("Password", UserValidationMessagesResource.PASSWORD_LETTER_NUMBER_INVALID);
                }
            });
    }
}
