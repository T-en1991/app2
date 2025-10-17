using System.Text.Json.Serialization;

namespace app2.Dtos.Common;

public class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public int Status { get; set; } // 0 = 失败, 1 = 成功

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    public static ApiResponse<T> Success(T data, string msg = "success", int code = 200)
        => new ApiResponse<T> { Status = 1, Msg = msg, Code = code, Data = data };

    public static ApiResponse<T> Fail(string msg, int code = 400, T data = default)
        => new ApiResponse<T> { Status = 0, Msg = msg, Code = code, Data = data };
}