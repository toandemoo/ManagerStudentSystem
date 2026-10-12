using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int RoleId);
        Task<Role> CreateRoleAsync(RoleDTO roleDTO);
        Task<RoleDTO> UpdateRoleAsync(int RoleId, RoleDTO roleDTO);
        Task<bool> DeleteRoleAsync(int RoleId);
    }
}