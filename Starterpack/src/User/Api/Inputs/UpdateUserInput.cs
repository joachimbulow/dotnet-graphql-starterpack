using FluentValidation;

namespace Spazw.User.Api.Inputs
{
    #pragma warning disable SA1313
    public record UpdateUserInput(Guid Id, string Name, string Email);

    public class UpdateUserInputValidator : AbstractValidator<UpdateUserInput>
    {
        public UpdateUserInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
