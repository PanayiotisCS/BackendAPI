using BackendApi.Responses;

namespace BackendAPI.Responses
{
    public class TokenResponse : BaseResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }

    public class ValidateRefreshTokenResponse : BaseResponse
    {
        public int UserId { get; set; }
    }

    public class SignupResponse : BaseResponse
    {
        public string Username { get; set; }
    }

    public class LogoutResponse : BaseResponse { }
}
