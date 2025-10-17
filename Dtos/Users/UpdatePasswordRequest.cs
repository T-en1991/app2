using System.Text.Json.Serialization;

namespace app2.Dtos.Users;

public class UpdatePasswordRequest
{
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }

    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}