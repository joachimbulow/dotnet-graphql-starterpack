using HotChocolate.Authorization;

namespace Starterpack.User.Domain.Models
{
    public class UserModel
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Email { get; init; }
    }
}