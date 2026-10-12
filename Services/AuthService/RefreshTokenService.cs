using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ILogger<RefreshTokenService> _logger;
        private readonly TaekwondoDbContext _context;
        private readonly IConfiguration _configuration;
        public RefreshTokenService(
            ILogger<RefreshTokenService> logger,
            TaekwondoDbContext context,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> CreateRefreshTokenAsync(int userId)
        {
            try
            {
                var newRefreshTK = new RefreshToken
                {
                    UserId = userId,
                    TokenHash = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                    CreateAt = DateTime.UtcNow,
                    ExpireIn = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:RefreshTokenValidityMins")),
                    RevoteAt = null
                };

                _context.RefreshTokens.Add(newRefreshTK);
                await _context.SaveChangesAsync();

                return newRefreshTK.TokenHash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating a refresh token with UserId = {userId}.");
                return null;
            }


        }

        public async Task<bool> DeleteRefreshTokenAsync(int refreshTokenId)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);
                if (refreshToken == null)
                {
                    return false;
                }

                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the refresh token with ID {refreshTokenId}.");
                return false;
            }
        }

        public async Task<RefreshTokenDTO> GetRefreshTokenByIdAsync(int userId)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.UserId == userId);

                if (refreshToken == null)
                {
                    return null;
                }

                return new RefreshTokenDTO
                {
                    UserId = refreshToken.UserId,
                    TokenHash = refreshToken.TokenHash,
                    CreateAt = refreshToken.CreateAt,
                    ExpireIn = refreshToken.ExpireIn,
                    RevoteAt = refreshToken.RevoteAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the refresh token with ID {userId}.");
                throw;
            }

        }

        public async Task<RefreshTokenDTO> GetRefreshTokenByTokenAsync(string refreshTK)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.TokenHash == refreshTK);

                if (refreshToken == null)
                {
                    return null;
                }

                return new RefreshTokenDTO
                {
                    RefreshTokenId = refreshToken.RefreshTokenId,
                    UserId = refreshToken.UserId,
                    TokenHash = refreshToken.TokenHash,
                    CreateAt = refreshToken.CreateAt,
                    ExpireIn = refreshToken.ExpireIn,
                    RevoteAt = refreshToken.RevoteAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the refresh token .");
                throw;
            }

        }

        public async Task<RefreshTokenDTO> UpdateRefreshTokenAsync(int refreshTokenId, RefreshTokenDTO refreshTokenDTO)
        {
            try
            {
                var existingRefreshTk = await _context.RefreshTokens.FindAsync(refreshTokenId);
                if (existingRefreshTk == null)
                {
                    return null;
                }

                existingRefreshTk.UserId = refreshTokenDTO.UserId;
                existingRefreshTk.TokenHash = refreshTokenDTO.TokenHash;

                _context.RefreshTokens.Update(existingRefreshTk);
                await _context.SaveChangesAsync();

                return refreshTokenDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the refresh token with ID {refreshTokenId}.");
                throw;
            }
        }
    }
}