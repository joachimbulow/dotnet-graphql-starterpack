using FluentValidation;

namespace Starterpack.Auth.Api.Inputs
{
#pragma warning disable SA1313
    public record LoginInput(string Email, string Password);

    // Validator
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}