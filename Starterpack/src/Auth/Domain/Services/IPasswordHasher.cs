namespace Starterpack.Auth.Domain.Services
{
    public interface IPasswordHasher
    {
        private const int DefaultSaltLength = 12;

        string HashPassword(string password, string salt);

        bool VerifyHashedPassword(string password, string hashedPassword);

        public string GenerateSalt(int length = DefaultSaltLength);
    }
}