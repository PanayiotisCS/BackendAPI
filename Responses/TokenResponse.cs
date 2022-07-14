using BackendApi.Responses;
using BackendAPI.Models;
using System.Text.Json;

namespace BackendAPI.Responses
{
    public class TokenResponse : BaseResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }

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

    public class GetUserResponse : BaseResponse
    {
        //public Claims User { get; internal set; }
        public String Student { get; set; }
    }
}
