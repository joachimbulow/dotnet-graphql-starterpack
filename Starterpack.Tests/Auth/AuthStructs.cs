using HotChocolate;
using Starterpack.Auth.Domain.Models;

public class LoginPayload : IPayload
{
    public TokensModel TokensModel { get; set; }
    public CommonError[] Errors { get; set; }
}