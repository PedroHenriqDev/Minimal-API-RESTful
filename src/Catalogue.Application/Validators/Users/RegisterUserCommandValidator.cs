using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Resources;
using FluentValidation;

namespace Catalogue.Application.Validators.Users;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandValidator()
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

        const int MAX_PASSWORD_LENGTH = 256;
        string passwordMessage = string.Format(UserValidationMessagesResource.PASSWORD_INVALID, MAX_PASSWORD_LENGTH);

        RuleFor(u => u.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MAX_PASSWORD_LENGTH)
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
