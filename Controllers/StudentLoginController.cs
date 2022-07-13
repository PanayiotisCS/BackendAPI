using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Requests;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentLoginController : ControllerBase
    {
        private readonly BackendContext _context;

        public StudentLoginController(BackendContext context)
        {
            _context = context;
        }

        // POST: api/StudentLogin
        [HttpPost]
        public ActionResult<User> StudentLogin(Login login)
        {

            var log = _context.Users.Where(x => x.Username.Equals(login.Username) && x.Password.Equals(login.Password)).FirstOrDefault();

            if (log == null)
            {
                return Ok(new { status = 401, isSuccess = false, message = "Invalid User", });
            }
            else
                return Ok(new { status = 200, isSuccess = true, message = "User Login succesfully", UserDetails = log });
        }

        [Route("registration")]
        [HttpPost]
        public async Task<Response> RegisterUser([FromQuery] User user)
        {
            if (_context.Users == null)
            {
                return new Response { Status = "Error", Message = "Invalid Data." };
            }

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                //if (UserExists(user.UserId) || RoleExists(role.Id) || StudentExists(student.StudentId))
                if (UserExists(user.Id))
                {
                    return new Response { Status = "Error", Message = "Invalid Data." };
                }
                else
                {
                    throw;
                }
            }
            return new Response { Status = "Success", Message = "Student Succesfully Saved." };
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
