using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Auth;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagerStudentSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            ILogger<AuthController> logger,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService
            )
        {
            _logger = logger;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var loginRespondDTO = await _jwtService.Login(loginRequestDTO);
            if (loginRequestDTO == null)
            {
                return BadRequest();
            }
            return Ok(loginRespondDTO);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO userDTO)
        {
            var loginRespondDTO = await _jwtService.Register(userDTO);
            if (loginRespondDTO == null)
            {
                return BadRequest();
            }
            return Ok(loginRespondDTO);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDTO refreshRequestDTO)
        {
            if (string.IsNullOrWhiteSpace(refreshRequestDTO.RefreshToken))
            {
                return BadRequest("Invalid Token");
            }

            var res = await _jwtService.ValidateRefreshToken(refreshRequestDTO.RefreshToken);

            return res is not null ? Ok(res) : Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequestDTO request)
        {
            var res = await _jwtService.Logout(request);

            if (!res)
            {
                return BadRequest(new
                {
                    message = "Invalid or already revoked refresh token"
                });
            }

            return Ok(new
            {
                message = "Logout successful"
            });
        }
    }
}