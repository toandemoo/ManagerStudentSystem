using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Auth;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IJwtService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LoginResponseDTO> Register(RegisterRequestDTO userDTO);
        Task<bool> Logout(RefreshRequestDTO refreshRequestDTO);
        Task<LoginResponseDTO> GenerateAccessToken(UserDTO userDTO);
        Task<string> GenerateRefreshToken(int userId);
        Task<LoginResponseDTO> ValidateRefreshToken(string Token);
        string HashRefreshToken(string refreshToken);

    }
}