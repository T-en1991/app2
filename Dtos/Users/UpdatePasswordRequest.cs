using System.Text.Json.Serialization;

namespace app2.Dtos.Users;

public class UpdatePasswordRequest
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "原密码不能为空")]
    [System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "原密码长度至少6位")]
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "新密码不能为空")]
    [System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "新密码长度至少6位")]
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}