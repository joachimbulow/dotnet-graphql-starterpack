using Starterpack.Common.Domain.Exceptions;

namespace Starterpack.Auth.Domain.Exceptions
{
    public class RefreshTokenExpiredException : BaseException
    {
        public RefreshTokenExpiredException()
            : base(message: "Refresh token has expired", code: "REFRESH_TOKEN_EXPIRED")
        {
        }
    }
}