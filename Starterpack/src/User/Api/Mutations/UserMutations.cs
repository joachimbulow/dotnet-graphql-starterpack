using FluentValidation;
using HotChocolate.Authorization;
using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Exceptions;
using Starterpack.User.Domain.Models;
using Spazw.User.Api.Inputs;

namespace Starterpack.User.Api.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class UserMutations
    {
        [Error(typeof(UserNotFoundException))]
        [Error(typeof(ValidationException))]
        [Authorize]
        public async Task<UserModel> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken, [Service] IUserService userService, [Service] IValidator<UpdateUserInput> validator, Lazy<UserModel> currentUser)
        {
            if (currentUser.Value.Id != input.Id)
            {
                throw new UserNotFoundException();
            }

            if (validator.Validate(input).IsValid == false)
            {
                throw new ValidationException(validator.Validate(input).Errors);
            }

            return await userService.UpdateUserAsync(input, cancellationToken);
        }

        [Error(typeof(ValidationException))]
        public async Task<UserModel> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken, [Service] IUserService userService, [Service] IValidator<CreateUserInput> validator)
        {
            if (validator.Validate(input).IsValid == false)
            {
                throw new ValidationException(validator.Validate(input).Errors);
            }

            return await userService.CreateUserAsync(input, cancellationToken);
        }
    }
}