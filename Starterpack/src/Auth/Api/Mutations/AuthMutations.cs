using FluentValidation;
using Starterpack.Auth.Api.Inputs;
using Starterpack.Auth.Domain.Exceptions;
using Starterpack.Auth.Domain.Models;
using Starterpack.Auth.Domain.Services;

namespace Starterpack.Auth.Api.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class AuthMutations
    {
        [Error(typeof(ValidationException))]
        [Error(typeof(InvalidLoginException))]
        public async Task<TokensModel> LoginAsync(LoginInput input, CancellationToken cancellationToken, [Service] IAuthService authService, [Service] IValidator<LoginInput> validator)
        {
            if (validator.Validate(input).IsValid == false)
            {
                throw new ValidationException(validator.Validate(input).Errors);
            }

            return await authService.LoginAsync(input, cancellationToken);
        }

        [Error(typeof(RefreshTokenNotFoundException))]
        [Error(typeof(RefreshTokenExpiredException))]
        public async Task<TokensModel> RefreshTokens(RefreshTokenInput input, CancellationToken cancellationToken, [Service] IAuthService authService)
        {
            return await authService.RefreshTokensAsync(input, cancellationToken);
        }
    }
}