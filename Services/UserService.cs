using app2.Dtos.Common;
using app2.Dtos.Users;
using app2.Models;
using app2.Repositories;

namespace app2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repo, ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<ApiResponse<UserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return ApiResponse<UserResponse>.Fail("用户名不能为空", 400);
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<UserResponse>.Fail("密码不能为空", 400);
            }

            var existing = await _repo.GetByNameAsync(request.Name);
            if (existing != null)
            {
                return ApiResponse<UserResponse>.Fail("用户名已存在", 409);
            }

            var user = new User
            {
                name = request.Name,
                password = request.Password, // 实际项目请做密码哈希
                is_delete = false
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            _logger.LogInformation("用户创建成功，ID: {UserId}, 用户名: {UserName}", user.id, user.name);

            var resp = MapToResponse(user);
            return ApiResponse<UserResponse>.Success(resp, "创建成功", 201);
        }

        public async Task<ApiResponse<UserResponse>> GetUserAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.Fail("用户不存在", 404);
            }

            var resp = MapToResponse(user);
            return ApiResponse<UserResponse>.Success(resp, "查询成功", 200);
        }

        public async Task<ApiResponse<object>> UpdatePasswordAsync(int id, UpdatePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword))
            {
                return ApiResponse<object>.Fail("原密码不能为空", 400);
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return ApiResponse<object>.Fail("新密码不能为空", 400);
            }
            if (request.OldPassword == request.NewPassword)
            {
                return ApiResponse<object>.Fail("新密码不能与原密码相同", 400);
            }

            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return ApiResponse<object>.Fail("用户不存在", 404);
            }
            if (user.password != request.OldPassword)
            {
                return ApiResponse<object>.Fail("原密码错误", 400);
            }

            user.password = request.NewPassword;
            await _repo.SaveChangesAsync();

            _logger.LogInformation("用户密码修改成功，用户ID: {UserId}, 用户名: {UserName}", user.id, user.name);

            return ApiResponse<object>.Success(null, "密码修改成功", 200);
        }

        public async Task<ApiResponse<object>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            if (request.Id <= 0)
            {
                return ApiResponse<object>.Fail("用户ID不合法", 400);
            }
            if (string.IsNullOrWhiteSpace(request.OldPassword))
            {
                return ApiResponse<object>.Fail("原密码不能为空", 400);
            }
            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return ApiResponse<object>.Fail("新密码不能为空", 400);
            }
            if (request.OldPassword == request.NewPassword)
            {
                return ApiResponse<object>.Fail("新密码不能与原密码相同", 400);
            }

            var user = await _repo.GetByIdAsync(request.Id);
            if (user == null)
            {
                return ApiResponse<object>.Fail("用户不存在", 404);
            }
            if (user.password != request.OldPassword)
            {
                return ApiResponse<object>.Fail("原密码错误", 400);
            }

            user.password = request.NewPassword;
            await _repo.SaveChangesAsync();

            _logger.LogInformation("用户密码修改成功（POST），用户ID: {UserId}, 用户名: {UserName}", user.id, user.name);

            return ApiResponse<object>.Success(null, "密码修改成功", 200);
        }

        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.id,
                Name = user.name,
                CreateTime = user.create_time,
                UpdateTime = user.update_time
            };
        }
    }
}