using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int UserId);
        Task<UserDTO> CreateUserAsync(UserDTO userDTO, int roleId = 1);
        Task<UserDTO> UpdateUserAsync(int UserId, UserDTO userDTO);
        Task<bool> DeleteUserAsync(int UserId);
    }
}