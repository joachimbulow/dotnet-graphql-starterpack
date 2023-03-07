// create an interface for a user service that has one function to get a user by id
using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Models;
using Spazw.User.Api.Inputs;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken);

    Task<UserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task<UserModel> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken);

    Task<UserModel> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken);
}