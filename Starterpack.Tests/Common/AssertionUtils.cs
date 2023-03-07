using GraphQL;
using Starterpack.Common.Domain.Exceptions;

public static class AssertionUtils
{
    // Since mutation errors are results of exceptions, I suppose we can assume which exception threw it
    public static void AssertExceptionError(IPayload payload, BaseException exception)
    {
        Assert.NotNull(payload);
        Assert.True(payload.Errors.Any(error => error.Code == exception.Code));
    }

    public static void AssertValidationError(IPayload payload, String propertyName)
    {
        Assert.NotNull(payload);
        Assert.True(payload.Errors.Any(error => error.Errors != null && error.Errors.Any(nestedError => nestedError.PropertyName == propertyName)));
    }
}