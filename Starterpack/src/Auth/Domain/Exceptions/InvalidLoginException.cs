using Starterpack.Common.Domain.Exceptions;

namespace Starterpack.Auth.Domain.Exceptions
{
    public class InvalidLoginException : BaseException
    {
        public InvalidLoginException()
            : base(message: "Wrong email or password", code: "INVALID_LOGIN")
        {
        }
    }
}