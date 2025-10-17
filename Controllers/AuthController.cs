using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using app2.Dtos.Common;
using app2.Dtos.Users;
using app2.Dtos.Auth;
using app2.Repositories;
using app2.Services;

namespace app2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ITokenService _tokens;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository repo, ITokenService tokens, ILogger<AuthController> logger)
        {
            _repo = repo;
            _tokens = tokens;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthTokenResponse>>> Login([FromBody] LoginRequest request)
        {
            var user = await _repo.GetByNameAsync(request.Name);
            if (user == null || user.password != request.Password)
            {
                return BadRequest(ApiResponse<AuthTokenResponse>.Fail("用户名或密码错误", 400));
            }

            var token = _tokens.CreateToken(user, "User");
            _logger.LogInformation("用户登录成功，ID: {UserId}, 用户名: {UserName}", user.id, user.name);

            return Ok(ApiResponse<AuthTokenResponse>.Success(token, "登录成功", 200));
        }
    }
}