using Microsoft.AspNetCore.Mvc;
using app2.Dtos.Users;
using app2.Dtos.Common;

namespace app2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly app2.Services.IUserService _userService;
    private readonly ILogger<UserController> _logger;
    
    public UserController(app2.Services.IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        if (result.Status == 0)
        {
            return StatusCode(result.Code, result);
        }
        return CreatedAtAction(nameof(GetUser), new { id = result.Data.Id }, result);
    }
    /// <summary>
    /// 获取用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUser(int id)
    {
        var result = await _userService.GetUserAsync(id);
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// 更新用户密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}/password")]
    public async Task<ActionResult<ApiResponse<object>>> UpdatePassword(int id, [FromBody] UpdatePasswordRequest request)
    {
        var result = await _userService.UpdatePasswordAsync(id, request);
        return StatusCode(result.Code, result);
    }
    /// <summary>
    /// 密码重置
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("password")]
    public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(request);
        return StatusCode(result.Code, result);
    }
}