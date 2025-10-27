using StoreManagement.API.Modules.Authentication.DTOs.Requests;
using StoreManagement.API.Modules.Authentication.DTOs.Responses;
using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Modules.Authentication.Mapper
{
    public class UserMapper
    {
        public  void RegisterUserMapper(User user,RegisterUserRequest request)
        {
            user.Email = request.Email;
            user.Username = request.UserName;
            user.PasswordHash = request.Password;
        }

        public UserResponse UserToUserResponse(User user) {
            return new UserResponse
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreateAt = user.CreatedAt,
                IsActive = user.IsActive,
                Phone = user.Phone

            };
        }
    }
}
