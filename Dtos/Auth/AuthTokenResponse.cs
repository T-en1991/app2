namespace app2.Dtos.Auth
{
    public class AuthTokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}