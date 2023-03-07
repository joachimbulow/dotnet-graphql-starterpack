using HotChocolate.Authorization;
using Starterpack.User.Domain.Models;

namespace Starterpack.User.Api.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class UserQueries
{
    public async Task<UserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken, [Service] IUserService userService)
    {
        return await userService.GetUserByIdAsync(id, cancellationToken);
    }

    [Authorize]
    public async Task<IEnumerable<UserModel>> GetUsers(CancellationToken cancellationToken, [Service] IUserService userService)
    {
        return await userService.GetUsersAsync(cancellationToken);
    }
}