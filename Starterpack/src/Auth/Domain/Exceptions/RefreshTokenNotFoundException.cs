using Starterpack.Common.Domain.Exceptions;

namespace Starterpack.Auth.Domain.Exceptions
{
    public class RefreshTokenNotFoundException : BaseException
    {
        public RefreshTokenNotFoundException()
            : base(message: "The refresh token was not found in the database", code: "REFRESH_TOKEN_NOT_FOUND")
        {
        }
    }
}