using FluentValidation;

namespace Starterpack.User.Api.Inputs
{
#pragma warning disable SA1313
    public record CreateUserInput(string Name, string Email, string Password);

    public class CreateUserInputValidator : AbstractValidator<CreateUserInput>
    {
        public CreateUserInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
