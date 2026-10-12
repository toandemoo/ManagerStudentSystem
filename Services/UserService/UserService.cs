using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.InterfaceServices;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class UserService : IUserService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IStudentService _studentService;
        public UserService(
            TaekwondoDbContext context,
            ILogger<UserService> logger,
            IStudentService studentService
            )
        {
            _context = context;
            _logger = logger;
            _studentService = studentService;
        }
        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO, int roleId = 5)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
                if (role == null)
                {
                    _logger.LogWarning("Role with ID {RoleId} not found. Assigning default role.", roleId);
                }

                userDTO.RoleId = roleId;

                var newuser = new User
                {
                    UserName = userDTO.UserName,
                    PasswordHash = userDTO.PasswordHash,
                    FullName = userDTO.FullName,
                    DateOfBirth = userDTO.DateOfBirth,
                    Gender = userDTO.Gender, // true for male, false for female
                    PhoneNumber = userDTO.PhoneNumber,
                    Email = userDTO.Email,
                    Address = userDTO.Address,
                    Roles = new List<Role>
                    {
                        role
                    }
                };

                _context.Users.Add(newuser);
                await _context.SaveChangesAsync();

                userDTO.UserId = newuser.UserId;

                await _studentService.CreateStudentAsync(newuser.UserId);

                return userDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int UserId)
        {
            try
            {
                var user = await _context.Users.FindAsync(UserId);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return false;
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(s => new UserDTO
            {
                UserId = s.UserId,
                UserName = s.UserName,
                PasswordHash = s.PasswordHash,
                FullName = s.FullName,
                DateOfBirth = s.DateOfBirth,
                Gender = s.Gender,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                Address = s.Address
            }).ToList();
        }

        public async Task<UserDTO> GetUserByIdAsync(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
                return null;

            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserDTO> UpdateUserAsync(int UserId, UserDTO userDTO)
        {
            var existingUser = await _context.Users.FindAsync(UserId);
            if (existingUser == null)
                return null;

            existingUser.UserName = userDTO.UserName;
            existingUser.PasswordHash = userDTO.PasswordHash;
            existingUser.FullName = userDTO.FullName;
            existingUser.DateOfBirth = userDTO.DateOfBirth;
            existingUser.Gender = userDTO.Gender;
            existingUser.PhoneNumber = userDTO.PhoneNumber;

            await _context.SaveChangesAsync();

            return userDTO;
        }
    }
}