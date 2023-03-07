using Starterpack.Common.Domain.Exceptions;

namespace Starterpack.User.Domain.Exceptions
{
    public class UserNotFoundException : BaseException
    {
        public UserNotFoundException()
            : base(message: "User with this id does not exist", code: "USER_NOT_FOUND")
        {
        }
    }
}