namespace StoreManagement.API.Common.Exceptions
{
    public class AppException : Exception
    {
      
        public IErrorCode ErrorCode { get; }
        public AppException(IErrorCode err):base(err.Message)
        {
            ErrorCode = err;
        }

    }
}
