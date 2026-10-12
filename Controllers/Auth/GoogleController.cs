using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class GoogleController : ControllerBase
    {
        private readonly ILogger<GoogleController> _logger;
        private readonly IUserService _userService;

        public GoogleController(ILogger<GoogleController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Google");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                                .FirstOrDefault()?.Claims
                                .Select(claim => new
                                {
                                    claim.Type,
                                    claim.Value
                                });

            var newuser = new UserDTO
            {
                FullName = claims.FirstOrDefault(c => c.Type == "name")?.Value,
                DateOfBirth = DateOnly.Parse(claims.FirstOrDefault(c => c.Type == "birthdate")?.Value ?? DateTime.MinValue.ToString()),
                Gender = claims.FirstOrDefault(c => c.Type == "gender")?.Value == "male",
                PhoneNumber = claims.FirstOrDefault(c => c.Type == "phone_number")?.Value,
            };

            await _userService.CreateUserAsync(newuser);

            return Ok(claims);
        }
    }
}