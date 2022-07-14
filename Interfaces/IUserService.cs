using BackendAPI.Models;
using BackendAPI.Requests;
using BackendAPI.Responses;

namespace BackendAPI.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(Login loginRequest);
        Task<SignupResponse> SignupAsync(SignupRequest signupRequest);
        Task<LogoutResponse> LogoutAsync(int userId);

        Task<GetUserResponse> GetUser(int userId);
    }
}
