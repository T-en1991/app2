using System.Text.Json.Serialization;

namespace app2.Dtos.Users;

public class ChangePasswordRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }

    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}