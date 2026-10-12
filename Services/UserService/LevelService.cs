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
    public class LevelService : ILevelsService
    {
        private readonly ILogger<LevelService> _logger;
        private readonly TaekwondoDbContext _context;
        public LevelService(ILogger<LevelService> logger, TaekwondoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<LevelDTO> CreateLevelAsync(LevelDTO level)
        {
            try
            {
                var newLevel = new Level
                {
                    LevelName = level.LevelName
                };

                _context.Levels.Add(newLevel);
                await _context.SaveChangesAsync();

                return new LevelDTO
                {
                    LevelName = newLevel.LevelName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new level.");
                return null;
            }
        }

        public async Task<bool> DeleteLevelAsync(int id)
        {
            try
            {
                var level = await _context.Levels.FindAsync(id);
                if (level == null)
                {
                    return false;
                }

                _context.Levels.Remove(level);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the level with ID {id}.");
                return false;
            }
        }

        public async Task<IEnumerable<LevelDTO>> GetAllLevelsAsync()
        {
            try
            {
                var levels = await _context.Levels.ToListAsync();
                return levels.Select(l => new LevelDTO { LevelName = l.LevelName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all levels.");
                throw;
            }
        }


        public async Task<LevelDTO> GetLevelByIdAsync(int id)
        {
            try
            {
                var level = await _context.Levels.FindAsync(id);
                if (level == null)
                {
                    return null;
                }

                return new LevelDTO
                {
                    LevelName = level.LevelName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the level with ID {id}.");
                throw;
            }
        }

        public async Task<LevelDTO> UpdateLevelAsync(int id, LevelDTO level)
        {
            try
            {
                var existingLevel = await _context.Levels.FindAsync(id);
                if (existingLevel == null)
                {
                    return null;
                }

                existingLevel.LevelName = level.LevelName;

                _context.Levels.Update(existingLevel);
                await _context.SaveChangesAsync();

                return new LevelDTO
                {
                    LevelName = existingLevel.LevelName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the level with ID {id}.");
                throw;
            }
        }
    }
}