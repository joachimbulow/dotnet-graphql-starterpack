namespace Starterpack.Common.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(string message, string code)
            : base(message)
        {
            this.Code = code;
        }

        public string Code { get; set; }
    }
}