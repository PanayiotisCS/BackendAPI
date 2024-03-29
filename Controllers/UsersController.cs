﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendAPI.Services;
using BackendAPI.Interfaces;
using BackendAPI.Requests;
using BackendAPI.Responses;
using BackendAPI.Models;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login loginRequest)
        {
            if(loginRequest == null)
            {
                return BadRequest(new TokenResponse
                {
                    Error = "Missing credentials",
                    ErrorCode = "L01"
                });
            }

            var loginResponse = await userService.LoginAsync(loginRequest);

            if (!loginResponse.Success)
            {
                return Unauthorized(new
                {
                    loginResponse.ErrorCode,
                    loginResponse.Error
                });
            }

            return Ok(loginResponse);
        }

        [HttpPost]
        [Route("refresh_token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null)
            {
                return BadRequest(new TokenResponse
                {
                    Error = "Missing refresh token details",
                    ErrorCode = "R01"
                });
            }

            var validateRefreshTokenResponse = await tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

            if (!validateRefreshTokenResponse.Success)
            {
                return UnprocessableEntity(validateRefreshTokenResponse);
            }

            var tokenResponse = await tokenService.GenerateTokensAsync(validateRefreshTokenResponse.UserId);

            return Ok(new { AccessToken = tokenResponse.Item1, Refreshtoken = tokenResponse.Item2 });
        }


        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup(SignupRequest signupRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
                if (errors.Any())
                {
                    return BadRequest(new TokenResponse
                    {
                        Error = $"{string.Join(",", errors)}",
                        ErrorCode = "S01"
                    });
                }
            }

            var signupResponse = await userService.SignupAsync(signupRequest);

            if (!signupResponse.Success)
            {
                return UnprocessableEntity(signupResponse);
            }

            return Ok(signupResponse.Username);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(int id)
        {
            var logout = await userService.LogoutAsync(id);

            if (!logout.Success)
            {
                return UnprocessableEntity(logout);
            }

            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {

            //if (UserID != id)
            //{
            //    return NotFound();
            //}
            var getUser = await userService.GetUser(id);

            //if (!getUser.Success)
            //{
            //    return UnprocessableEntity(getUser);
            //}

            ////var checkToken = getUser.User.RefreshTokens.Where(t => t.TokenHash == token);
            ////if (checkToken == null)
            ////{
            ////    return NotFound();
            ////}
            return Ok(getUser.Student);
        }
    }
}
