namespace StoreManagement.API.Common.Exceptions
{
    public interface IErrorCode
    {
        int StatusCode { get; }
        string Message { get; }
        bool Success { get; }
    }
}
