using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDTO> GetRefreshTokenByIdAsync(int userId);
        Task<RefreshTokenDTO> GetRefreshTokenByTokenAsync(string refreshTK);
        Task<string> CreateRefreshTokenAsync(int userId);
        Task<RefreshTokenDTO> UpdateRefreshTokenAsync(int refreshTokenId, RefreshTokenDTO refreshTokenDTO);
        Task<bool> DeleteRefreshTokenAsync(int refreshTokenId);
    }
}