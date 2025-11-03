using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Users.Dtos.Response;

namespace StoreManagement.API.Modules.Users.Mapper
{
    public static class UserMapper
    {
        public static UserResponse UserToUserResponse(Account user)
        {
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
