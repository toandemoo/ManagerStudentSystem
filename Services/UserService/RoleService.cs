using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<RoleService> _logger;
        public RoleService(TaekwondoDbContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Role> CreateRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                var role = new Role
                {
                    RoleName = roleDTO.RoleName,
                    Description = roleDTO.Description
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new role.");
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(int RoleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(RoleId);
                if (role == null)
                {
                    return false;
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID {RoleId}.");
                throw;
            }
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return roles.Select(r => new RoleDTO
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all roles.");
                throw;
            }
        }

        public async Task<RoleDTO> GetRoleByIdAsync(int RoleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(RoleId);
                if (role == null)
                {
                    return null;
                }

                return new RoleDTO
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the role with ID {RoleId}.");
                throw;
            }
        }

        public async Task<RoleDTO> UpdateRoleAsync(int RoleId, RoleDTO roleDTO)
        {
            try
            {
                var role = await _context.Roles.FindAsync(RoleId);
                if (role == null)
                {
                    return null;
                }

                role.RoleName = roleDTO.RoleName;
                role.Description = roleDTO.Description;

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return new RoleDTO
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the role with ID {RoleId}.");
                throw;
            }
        }
    }
}