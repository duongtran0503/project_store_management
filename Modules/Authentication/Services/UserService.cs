namespace StoreManagement.API.Modules.Authentication.Services
{
    using Microsoft.AspNetCore.Authorization;
    using StoreManagement.API.Common.Entities;
    using StoreManagement.API.Common.Exceptions;
    using StoreManagement.API.Modules.Authentication.Constants;
    using StoreManagement.API.Modules.Authentication.DTOs.Requests;
    using StoreManagement.API.Modules.Authentication.DTOs.Responses;
    using StoreManagement.API.Modules.Authentication.ErrorCodes;
    using StoreManagement.API.Modules.Authentication.Mapper;
    using StoreManagement.API.Modules.Authentication.Repository;
    using StoreManagement.API.Modules.Authentication.Utils;
    using StoreManagement.API.Shared.Entities;

        public class UserService
    {
                private readonly UserRepository _userRepository;

                private readonly UserMapper _userMapper;

                private readonly JwtService _jwtService;
                private readonly AuthTokenService _authTokenService;

                public UserService(UserRepository userRepository, UserMapper userMapper
            , JwtService jwtService,AuthTokenService authTokenService
            )
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
            _jwtService = jwtService;
            _authTokenService = authTokenService;
        }

                public async Task<AuthenticationResponse> RegisterUser(RegisterUserRequest request)
        {
            var checkUser = await _userRepository.CheckUserByEmailAsync(request.Email);
            if (checkUser)
            {
                throw new AppException(AuthErrorCode.AccountExisted);
            }
            var user = new Account();
            user.RoleName = Roles.STAFF.ToString();
            _userMapper.RegisterUserMapper(user, request);
            user.PasswordHash = PasswordHelper.HashPassword(request.Password);
            var userInfo = await _userRepository.RegisterAsync(user);
            var jwtToken = _jwtService.GenerateToken(new UserInfoClaim
            {
                UserName = userInfo.Username,
                Email = userInfo.Email,
                Id = userInfo.Id,
                Role = userInfo.RoleName,
            });

            return new AuthenticationResponse { AccessToken = jwtToken.AccessToken, RefreshToken = jwtToken.RefreshToken };
        }

        public async Task<AuthenticationResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
                throw new AppException(AuthErrorCode.UserNotFound);

            var checkPassword = PasswordHelper.VerifyPassword(request.Password, user.PasswordHash);
            
            if(!checkPassword)
            {
                throw new AppException(AuthErrorCode.LoginFail);
            }

            var jwtToken = _jwtService.GenerateToken(new UserInfoClaim
            {
                UserName = user.Username,
                Email = user.Email,
                Id = user.Id,
                Role = user.RoleName,
            });

            return new AuthenticationResponse { AccessToken=jwtToken.AccessToken,RefreshToken = jwtToken.RefreshToken };
        }

   
        public async Task<UserResponse> GetProfile()
        {
            var userId = _authTokenService.GetCurrentUserId();
            var user = await _userRepository.GetUserById(userId);
            return _userMapper.UserToUserResponse(user);
        }
    }

}
