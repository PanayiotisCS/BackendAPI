using Microsoft.EntityFrameworkCore;
using BackendAPI.Helpers;
using BackendAPI.Interfaces;
using BackendAPI.Requests;
using BackendAPI.Responses;
using BackendAPI.Models;

namespace BackendAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly BackendContext _context;

        public UserService(BackendContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> LoginAsync(Login loginRequest)
        {
            var user = _context.Users.SingleOrDefault(user => user.Username == loginRequest.Username);

            if (user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Username not found",
                    ErrorCode = "L02"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Invalid Password",
                    ErrorCode = "L03"
                };
            }

            var token = await System.Threading.Tasks.Task.Run(() => _tokenService.GenerateTokensAsync(user.Id));

            return new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }

        public async Task<LogoutResponse> LogoutAsync(int userId)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == userId);

            if (refreshToken == null)
            {
                return new LogoutResponse { Success = true };
            }

            _context.RefreshTokens.Remove(refreshToken);

            var saveResponse = await _context.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new LogoutResponse { Success = true };
            }

            return new LogoutResponse { Success = false, Error = "Unable to logout user", ErrorCode = "L04" };
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest signupRequest)
        {
            var existinUser = await _context.Users.SingleOrDefaultAsync(user => user.Username == signupRequest.Username);

            if (existinUser != null)
            {
                return new SignupResponse
                {
                    Success = false,
                    Error = "User already exist with the same email",
                    ErrorCode = "S02"
                };
            }
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var Hpassword = BCrypt.Net.BCrypt.HashPassword(signupRequest.Password, salt);

            var user = new User
            {
                Username = signupRequest.Username,
                Password = Hpassword,
                PasswordSalt = salt,
                RoleId = 1
            };

            await _context.Users.AddAsync(user);

            user.Students?.Add(new Student
            {
                UserId = user.Id,
                Fname = signupRequest.Fname,
                Lname = signupRequest.Lname,
                Email = signupRequest.Email,
                StudentNumber = signupRequest.StudentNumber,
                Phone = signupRequest.Phone
            });

            var saveResponse = await _context.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SignupResponse { Success = true, Username = user.Username };
            }

            return new SignupResponse
            {
                Success = false,
                Error = "Unable to save the user",
                ErrorCode = "S05"
            };
        }
    }
}
