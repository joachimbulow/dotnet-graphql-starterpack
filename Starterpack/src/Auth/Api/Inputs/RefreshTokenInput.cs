using FluentValidation;

namespace Starterpack.Auth.Api.Inputs
{
#pragma warning disable SA1313
    public record RefreshTokenInput(string Token, string RefreshToken);
}