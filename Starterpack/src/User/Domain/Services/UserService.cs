using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Models;
using Spazw.User.Api.Inputs;

namespace Starterpack.User.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByIdAsync(id, cancellationToken);
    }

    public async Task<UserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByEmailAsync(email, cancellationToken);
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _userRepository.GetUsersAsync(cancellationToken);
    }

    public Task<UserModel> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken)
    {
        return _userRepository.UpdateUserAsync(input, cancellationToken);
    }

    public Task<UserModel> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        return _userRepository.CreateUserAsync(input, cancellationToken);
    }
}