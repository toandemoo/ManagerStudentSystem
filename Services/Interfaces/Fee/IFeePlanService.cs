using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IFeePlanService
    {
        Task<IEnumerable<FeePlanDTO>> GetAllFeePlansAsync();
        Task<FeePlanDTO> GetFeePlanByIdAsync(int id);
        Task<FeePlanDTO> CreateFeePlanAsync(FeePlanDTO feePlanDto);
        Task<FeePlanDTO> UpdateFeePlanAsync(int id, FeePlanDTO feePlanDto);
        Task<bool> DeleteFeePlanAsync(int id);
    }
}