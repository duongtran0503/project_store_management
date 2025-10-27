using StoreManagement.API.Modules.Authentication.Constants;

namespace StoreManagement.API.Modules.Authentication.Services
{
    public class AuthTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtService _tokenService;

        public AuthTokenService(IHttpContextAccessor httpContextAccessor, JwtService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public string GetCurrentUserId()
        {
            var userInfo = GetCurrentUserInfo();
            return userInfo?.UserId;  
        }

        public string GetCurrentUsername()
        {
            var userInfo = GetCurrentUserInfo();
            return userInfo?.Username; 
        }

        public string GetCurrentUserEmail()
        {
            var userInfo = GetCurrentUserInfo();
            return userInfo?.Email;  
        }

        public string GetCurrentUserRole()
        {
            var userInfo = GetCurrentUserInfo();
            return userInfo?.Role;

        }
        public TokenInfo GetCurrentUserInfo()
        {
            return _tokenService.GetTokenInfo(_httpContextAccessor.HttpContext);
        }

        public bool IsAdmin()
        {
            var role = GetCurrentUserRole();
            return role?.ToUpper() ==Roles.ADMIN.ToString(); 
        }

        public bool IsStaff()
        {
            var role = GetCurrentUserRole();
            return role?.ToUpper() == Roles.STAFF.ToString();  
        }

        public bool IsAuthenticated()
        {
            var token = _tokenService.GetTokenFromHttpContext(_httpContextAccessor.HttpContext);
            return !string.IsNullOrEmpty(token) && _tokenService.ValidateToken(token) != null;
        }

        // 🔥 KIỂM TRA USER CÓ PHẢI LÀ CHỦ SỞ HỮU KHÔNG
        public bool IsOwner(string userId)
        {
            var currentUserId = GetCurrentUserId();
            return currentUserId == userId;
        }

        // 🔥 KIỂM TRA QUYỀN TRUY CẬP
        public bool HasAccess(string userId)
        {
            return IsOwner(userId) || IsAdmin();
        }

        // 🔥 KIỂM TRA ROLE CỤ THỂ
        public bool HasRole(string role)
        {
            var currentRole = GetCurrentUserRole();
            return currentRole?.ToUpper() == role.ToUpper();
        }
    }
}
