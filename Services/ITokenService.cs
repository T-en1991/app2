namespace app2.Services
{
    public interface ITokenService
    {
        app2.Dtos.Auth.AuthTokenResponse CreateToken(app2.Models.User user, string role = "User");
    }
}