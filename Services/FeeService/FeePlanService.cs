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
    public class FeePlanService : IFeePlanService
    {
        private readonly ILogger<FeePlanService> _logger;
        private readonly TaekwondoDbContext _context;

        public FeePlanService(ILogger<FeePlanService> logger, TaekwondoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<FeePlanDTO> CreateFeePlanAsync(FeePlanDTO feePlanDto)
        {
            try
            {
                var feePlan = new FeePlan
                {
                    FeePlanName = feePlanDto.FeePlanName,
                    FeeAmountMonthly = feePlanDto.FeeAmountMonthly,
                    DisCountPercentage = feePlanDto.DisCountPercentage,
                    Description = feePlanDto.Description
                };

                _context.FeePlans.Add(feePlan);
                await _context.SaveChangesAsync();

                return new FeePlanDTO
                {
                    FeePlanName = feePlan.FeePlanName,
                    FeeAmountMonthly = feePlan.FeeAmountMonthly,
                    DisCountPercentage = feePlan.DisCountPercentage,
                    Description = feePlan.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new fee plan.");
                return null; // or throw a custom exception
            }
        }

        public async Task<bool> DeleteFeePlanAsync(int id)
        {
            try
            {
                var feePlan = await _context.FeePlans.FindAsync(id);
                if (feePlan == null)
                {
                    return false; // Fee plan not found
                }

                _context.FeePlans.Remove(feePlan);
                await _context.SaveChangesAsync();
                return true; // Successfully deleted
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the fee plan with ID {id}.");
                return false; // or throw a custom exception
            }
        }

        public async Task<IEnumerable<FeePlanDTO>> GetAllFeePlansAsync()
        {
            try
            {
                var feePlans = await _context.FeePlans.ToListAsync();
                return feePlans.Select(fp => new FeePlanDTO
                {
                    FeePlanName = fp.FeePlanName,
                    FeeAmountMonthly = fp.FeeAmountMonthly,
                    DisCountPercentage = fp.DisCountPercentage,
                    Description = fp.Description
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all fee plans.");
                return Enumerable.Empty<FeePlanDTO>(); // or throw a custom exception
            }
        }

        public async Task<FeePlanDTO> GetFeePlanByIdAsync(int id)
        {
            try
            {
                var feePlan = await _context.FeePlans.FindAsync(id);
                if (feePlan == null)
                {
                    return null; // Fee plan not found
                }

                return new FeePlanDTO
                {
                    FeePlanName = feePlan.FeePlanName,
                    FeeAmountMonthly = feePlan.FeeAmountMonthly,
                    DisCountPercentage = feePlan.DisCountPercentage,
                    Description = feePlan.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the fee plan with ID {id}.");
                return null; // or throw a custom exception
            }
        }

        public async Task<FeePlanDTO> UpdateFeePlanAsync(int id, FeePlanDTO feePlanDto)
        {
            try
            {
                var feePlan = await _context.FeePlans.FindAsync(id);
                if (feePlan == null)
                {
                    return null; // Fee plan not found
                }

                feePlan.FeePlanName = feePlanDto.FeePlanName;
                feePlan.FeeAmountMonthly = feePlanDto.FeeAmountMonthly;
                feePlan.DisCountPercentage = feePlanDto.DisCountPercentage;
                feePlan.Description = feePlanDto.Description;

                await _context.SaveChangesAsync();

                return new FeePlanDTO
                {
                    FeePlanName = feePlan.FeePlanName,
                    FeeAmountMonthly = feePlan.FeeAmountMonthly,
                    DisCountPercentage = feePlan.DisCountPercentage,
                    Description = feePlan.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the fee plan with ID {id}.");
                return null; // or throw a custom exception
            }
        }
    }
}