using app2.Dtos.Common;
using app2.Dtos.Users;

namespace app2.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponse>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<UserResponse>> GetUserAsync(int id);
        Task<ApiResponse<object>> UpdatePasswordAsync(int id, UpdatePasswordRequest request);
        Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request);
    }
}