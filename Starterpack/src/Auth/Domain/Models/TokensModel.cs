namespace Starterpack.Auth.Domain.Models
{
    public class TokensModel
    {
        public string AccessToken { get; init; }

        public string RefreshToken { get; init; }
    }
}