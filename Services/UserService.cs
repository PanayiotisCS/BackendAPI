using Microsoft.EntityFrameworkCore;
using BackendAPI.Helpers;
using BackendAPI.Interfaces;
using BackendAPI.Requests;
using BackendAPI.Responses;
using BackendAPI.Models;
using System.Security.Claims;
using System.Text.Json;
using Newtonsoft.Json;

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
                    Error = "Wrong credentials",
                    ErrorCode = "L02"
                };
            }

            var passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.PasswordSalt));

            if (user.Password != passwordHash)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Wrong Credentials",
                    ErrorCode = "L03"
                };
            }

            var token = await System.Threading.Tasks.Task.Run(() => _tokenService.GenerateTokensAsync(user.Id));

            if (loginRequest.Role == "Student")
            {
                var student = _context.Students.SingleOrDefault(s => s.UserId == user.Id);

                if (student == null)
                {
                    return new TokenResponse
                    {
                        Success = false,
                        Error = "Wrong Credentials",
                        ErrorCode = "L04"
                    };
                }

                return new TokenResponse
                {
                    Success = true,
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2,
                    Role = "student",
                    UserId = user.Id,
                    Fname = student.Fname,
                    Lname = student.Lname,
                    Email = student.Email
                };
            }
            var admin = _context.Admins.SingleOrDefault(s => s.UserId == user.Id);

            if (admin == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Wrong Credentials",
                    ErrorCode = "L04"
                };
            }

            return new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                Role = "admin",
                UserId = user.Id,
                Fname = admin.Fname,
                Lname = admin.Lname,
                Email = admin.Email
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
            var salt = PasswordHelper.GetSecureSalt();
            var Hpassword = PasswordHelper.HashUsingPbkdf2(signupRequest.Password, salt);

            if (signupRequest.Type.Equals("Admin"))
            {
                var user = new User
                {
                    Username = signupRequest.Username,
                    Password = Hpassword,
                    PasswordSalt = Convert.ToBase64String(salt),
                    RoleId = 2
                };

                await _context.Users.AddAsync(user);

                user.Admins?.Add(new Admin
                {
                    UserId = user.Id,
                    Fname = signupRequest.Fname,
                    Lname = signupRequest.Lname,
                    Email = signupRequest.Email,
                });

                var saveResponse = await _context.SaveChangesAsync();

                if (saveResponse >= 0)
                {
                    return new SignupResponse { Success = true, Username = user.Username };
                }
            }
            else
            {
                var user = new User
                {
                    Username = signupRequest.Username,
                    Password = Hpassword,
                    PasswordSalt = Convert.ToBase64String(salt),
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
            }
            
            return new SignupResponse
            {
                Success = false,
                Error = "Unable to save the user",
                ErrorCode = "S05"
            };
        }


        public async Task<GetUserResponse> GetUser(int userId)
        {
            var results = _context.Students
                .FromSqlInterpolated($"EXECUTE [dbo].[GET_USER] @user_id ={userId}")
                .ToList();
        //var user = _context.Users.SingleOrDefault(user => user.Id == userId);
     
            //if (user == null)
            //{
            if (results.Count <= 0)
                return new GetUserResponse
                {
                    Success = false,
                    Error = "Something went wrong",
                    ErrorCode = "S06"
                };
            //}
            //var student = _context.Students.SingleOrDefault(s => s.UserId == userId);

            //if (student == null)
            //{
            //    return new GetUserResponse
            //    {
            //        Success = false,
            //        Error = "Something went wrong",
            //        ErrorCode = "S06"
            //    };
            //}

            return new GetUserResponse
            {
                Success = true,
                //Student = JsonConvert.SerializeObject(results)
                Student = JsonConvert.SerializeObject(results)
            };

            //_context.SqlQuery<YourEntityType>("storedProcedureName",params);
        }
    }
}
