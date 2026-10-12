using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface ILevelsService
    {
        Task<IEnumerable<LevelDTO>> GetAllLevelsAsync();
        Task<LevelDTO> GetLevelByIdAsync(int id);
        Task<LevelDTO> CreateLevelAsync(LevelDTO level);
        Task<LevelDTO> UpdateLevelAsync(int id, LevelDTO level);
        Task<bool> DeleteLevelAsync(int id);
    }
}