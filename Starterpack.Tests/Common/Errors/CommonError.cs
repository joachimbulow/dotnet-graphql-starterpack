///
///  This class is merely for json deserialization of the implicitly created error types
///
public class CommonError
{
    public string Message { get; set; }
    public string? Code { get; set; }
    public ValidationFailure[]? Errors { get; set; }
}