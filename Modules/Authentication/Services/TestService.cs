using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Authentication.ErrorCodes;

namespace StoreManagement.API.Modules.Authentication.Services
{
    public class TestService
    {
        public TestService() { }

        public String Hello()
        {
            throw new AppException(AuthErrorCode.LoginFail); 
            return "hello";
        }
    }
}
