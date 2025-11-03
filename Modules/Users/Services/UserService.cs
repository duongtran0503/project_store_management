
using StoreManagement.API.Modules.Authentication.Services;
using StoreManagement.API.Modules.Users.Dtos.Response;
using StoreManagement.API.Modules.Users.Mapper;
using StoreManagement.API.Modules.Users.Repository;

namespace StoreManagement.API.Modules.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly AuthTokenService _authTokenService;
        public UserService(UserRepository userRepository,AuthTokenService authTokenService) { 
          _userRepository = userRepository;
            _authTokenService = authTokenService;
        }

        public async Task<UserResponse> GetProfile()
        {
            var userId = _authTokenService.GetCurrentUserId();
            var user = await _userRepository.GetUserById(userId);
            return UserMapper.UserToUserResponse(user);
        }
    }
}
