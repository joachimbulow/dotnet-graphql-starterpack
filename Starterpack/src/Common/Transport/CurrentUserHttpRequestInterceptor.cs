using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Starterpack.User.Domain.Models;

namespace Common.Transport
{
    public sealed partial class CurrentUserHttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override async ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, IQueryRequestBuilder builder, CancellationToken cancellationToken)
        {
            var userIdClaim = context.User.FindFirst("userId");
            if (userIdClaim != null)
            {
                Lazy<UserModel> user = new Lazy<UserModel>(() => context.RequestServices.GetRequiredService<IUserService>().GetUserByIdAsync(Guid.Parse(userIdClaim.Value), cancellationToken).Result);
                builder.SetGlobalState(nameof(UserModel), user);
            }

            await base.OnCreateAsync(context, requestExecutor, builder, cancellationToken);
        }
    }
}