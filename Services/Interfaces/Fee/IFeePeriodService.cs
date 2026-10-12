using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos.Fee;

namespace ManagerStudentSystem.Services.Interfaces.Fee
{
    public interface IFeePeriodService
    {
        Task<FeePeriodDTO> CreateFeePeriodAsync(FeePeriodRequestDTO feePeriodDTO);
        Task<bool> DeleteFeePeriodAsync(int feePeriodId);
        Task<FeePeriodDTO> GetFeePeriodByIdAsync(int feePeriodId);
        Task<IEnumerable<FeePeriodDTO>> GetAllFeePeriodsAsync();
        Task<FeePeriodDTO> UpdateFeePeriodAsync(int feePeriodId, FeePeriodDTO feePeriodDTO);
    }
}