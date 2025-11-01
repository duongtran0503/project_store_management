using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Authentication.DTOs.Requests;
using StoreManagement.API.Modules.Authentication.DTOs.Responses;

namespace StoreManagement.API.Modules.Authentication.Mapper
{
    public class UserMapper
    {
        public  void RegisterUserMapper(Account user,RegisterUserRequest request)
        {
            user.Email = request.Email;
            user.Username = request.UserName;
            user.PasswordHash = request.Password;
        }

        public UserResponse UserToUserResponse(Account user) {
            return new UserResponse
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.RoleName,
                CreateAt = user.CreatedAt,
                IsActive = user.IsActive,
                Phone = user.Phone

            };
        }
    }
}
