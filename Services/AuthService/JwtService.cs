using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Auth;
using ManagerStudentSystem.InterfaceServices;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ManagerStudentSystem.Services
{
    public class JwtService : IJwtService
    {
        private readonly ILogger<JwtService> _logger;
        private readonly TaekwondoDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public JwtService(
            ILogger<JwtService> logger,
            TaekwondoDbContext context,
            IConfiguration configuration,
            IStudentService studentService,
            IUserService userService,
            IRefreshTokenService refreshTokenService
            )
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _studentService = studentService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserName == loginRequestDTO.UserName && s.PasswordHash == loginRequestDTO.PassWord);
            if (user == null)
            {
                return null;
            }

            var userDTO = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                UserName = user.UserName,
            };

            var expiration = _configuration.GetValue<int>("Jwt:AccessTokenValidityMins");
            var timespan = DateTime.UtcNow.AddMinutes(expiration);

            return await GenerateAccessToken(userDTO);
        }

        public async Task<LoginResponseDTO> Register(RegisterRequestDTO userDTO)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDTO.UserName);
            if (existingUser != null)
            {
                return null;
            }

            var user = new UserDTO
            {
                UserName = userDTO.UserName,
                PasswordHash = userDTO.PasswordHash,
                FullName = userDTO.FullName,
                DateOfBirth = userDTO.DateOfBirth,
                Gender = userDTO.Gender,
                Email = userDTO.Email
            };

            var newUser = await _userService.CreateUserAsync(user);

            return await GenerateAccessToken(newUser);
        }

        public async Task<LoginResponseDTO> GenerateAccessToken(UserDTO userDTO)
        {
            try
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var signingkey = _configuration["Jwt:Key"];
                var expiration = _configuration.GetValue<int>("Jwt:AccessTokenValidityMins");
                var timespan = DateTime.UtcNow.AddMinutes(expiration);

                var tokenJwtDecription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userDTO.FullName),
                        new Claim(ClaimTypes.NameIdentifier, userDTO.UserId.ToString())
                    }),
                    Issuer = issuer,
                    Audience = audience,
                    Expires = timespan,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingkey)), SecurityAlgorithms.HmacSha256)
                };

                var tokenJwtHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenJwtHandler.CreateToken(tokenJwtDecription);
                var accessToken = tokenJwtHandler.WriteToken(securityToken);
                var refreshToken = await GenerateRefreshToken(userDTO.UserId);

                return new LoginResponseDTO
                {
                    UserName = userDTO.UserName,
                    AccessToken = accessToken,
                    ExpireIn = (int)timespan.Subtract(DateTime.UtcNow).TotalSeconds,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Not Found UserId = {userDTO.UserId}");
                throw;
            }

        }

        public async Task<string> GenerateRefreshToken(int userId)
        {

            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(userId);

            return refreshToken;
        }

        public async Task<LoginResponseDTO> ValidateRefreshToken(string token)
        {
            try
            {
                var refreshToken = await _refreshTokenService.GetRefreshTokenByTokenAsync(token);

                if (refreshToken == null || refreshToken.ExpireIn <= DateTime.UtcNow)
                {
                    Console.WriteLine("Refresh token is invalid or expired.");
                    return null;
                }

                var user = await _userService.GetUserByIdAsync(refreshToken.UserId);

                if (user == null)
                {
                    return null;
                }

                await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken.RefreshTokenId);

                return await GenerateAccessToken(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validate the refresh token .");
                throw;
            }
        }

        public string HashRefreshToken(string refreshToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

            return Convert.ToBase64String(bytes);
        }

        public async Task<bool> Logout(RefreshRequestDTO refreshRequestDTO)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenByTokenAsync(refreshRequestDTO.RefreshToken);
            if (refreshToken == null)
            {
                return false;
            }

            await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken.RefreshTokenId);

            return true;
        }
    }
}