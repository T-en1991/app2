namespace app2.Dtos.Users;

public class CreateUserRequest
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "用户名不能为空")]
    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "用户名长度不能超过100")]
    public string Name { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "密码不能为空")]
    [System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "密码长度至少6位")]
    [System.ComponentModel.DataAnnotations.StringLength(255, ErrorMessage = "密码长度不能超过255")]
    public string Password { get; set; }
}