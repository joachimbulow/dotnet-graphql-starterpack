using Starterpack.User.Api.Resolvers;
using Starterpack.User.Domain.Models;

namespace Starterpack.User.Api.Types
{
    public class UserType : ObjectType<UserModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UserModel> descriptor)
        {
            descriptor.Description("Represent a user in the system.");

            // descriptor
            //     .Field(u => u.allUsers)
            //     .ResolveWith<UserResolver>(u => u.GetAllUsers(default!, default!));
        }
    }
}