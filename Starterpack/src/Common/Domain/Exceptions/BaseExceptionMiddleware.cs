using HotChocolate.Resolvers;

namespace Starterpack.Common.Domain.Exceptions
{
    public class BaseExceptionMiddleware
    {
        private readonly FieldDelegate _nextDelegate;

        public BaseExceptionMiddleware(FieldDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            try
            {
                await _nextDelegate(context);
            }
            catch (BaseException ex)
            {
                IError err = ErrorBuilder.New()
                    .SetMessage(ex.Message)
                    .SetCode(ex.Code)
                    .Build();

                context.ReportError(err);
            }
        }
    }
}