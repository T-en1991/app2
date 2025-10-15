using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app2.Models;

namespace app2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(AppDbContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("用户名不能为空");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("密码不能为空");
            }

            // 检查用户名是否已存在 - 使用EF Core的FirstOrDefaultAsync
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.name == request.Name && !u.is_delete);
            
            if (existingUser != null)
            {
                return Conflict("用户名已存在");
            }

            // 创建新用户 - 时间戳会自动设置
            var user = new User
            {
                name = request.Name,
                password = request.Password, // 注意：实际项目中应该对密码进行哈希处理
                is_delete = false
                // create_time 和 update_time 会在SaveChangesAsync时自动设置
            };

            // 使用EF Core添加和保存
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("用户创建成功，ID: {UserId}, 用户名: {UserName}", user.id, user.name);

            // 返回创建的用户（不包含密码）
            var userResponse = new UserResponse
            {
                Id = user.id,
                Name = user.name,
                CreateTime = user.create_time,
                UpdateTime = user.update_time
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, userResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建用户时发生错误");
            return StatusCode(500, "服务器内部错误");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        // 使用EF Core的FirstOrDefaultAsync查询
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.id == id && !u.is_delete);

        if (user == null)
        {
            return NotFound("用户不存在");
        }

        var userResponse = new UserResponse
        {
            Id = user.id,
            Name = user.name,
            CreateTime = user.create_time,
            UpdateTime = user.update_time
        };

        return userResponse;
    }
}

// 请求模型
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
}

// 响应模型
public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}