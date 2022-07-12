using Microsoft.AspNetCore.Mvc;
using BackendAPI.Models;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        
        private readonly BackendContext _context;

        public AuthController(BackendContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<Response> Register(UserDto request)
        {
            if(_context.Users.Any(u => u.Username == request.Username))
            {
                return new Response { Status = "Error", Message = "Username taken." };
            }
            if(_context.Students.Any(e => e.Email == request.Email))
            {
                return new Response { Status = "Error", Message = "This email already exist." };
            }

            User user = new User();
            user.Username = request.Username;
            user.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password, user.PasswordSalt);
            user.RoleId = 1;

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return new Response { Status = "Error", Message = "User already exists." };
                }
                else
                {
                    throw;
                }
            }

            Student student = new Student();

            student.UserId = user.Id;
            student.Fname = request.Fname;
            student.Lname = request.Lname;
            student.Email = request.Email;
            student.StudentNumber = request.StudentNumber;
            student.Sex = request.Sex;
            student.Caddress = request.Caddress;
            student.CaddressCity = request.CaddressCity;
            student.CaddressNumber = request.CaddressNumber;
            student.CaddressPost = request.CaddressPost;
            student.Phone = request.Phone;

            _context.Students.Add(student);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (StudentExists(user.Id))
                {
                    return new Response { Status = "Error", Message = "User already exists." };
                }
                else
                {
                    throw;
                }
            }
            return new Response { Status = "Success", Message = "Success request" };
        }

        [HttpPost("login")]
        public async Task<Response> Login(Login request)
        {
            var user = await _context.Users.Where(x => x.Username.Equals(request.Username)).FirstOrDefaultAsync();
            
            if (user == null)
            {
                return new Response { Status = "Error", Message = "Something went wrong." };
            }
            if (!_context.Users.Any(u => u.Username == request.Username))
            {
                return new Response { Status = "Error", Message = "Wrong credentials." };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
            {
                return new Response { Status = "Error", Message = "Wrong credentials." };
            }

            string token = CreateToken(request);
            return new Response { Status = "Success", Message = token };
        }

        private string CreateToken(Login user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("secret_key_token"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
