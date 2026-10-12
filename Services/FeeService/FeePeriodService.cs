using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos.Fee;
using ManagerStudentSystem.InterfaceServices;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces.Fee;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services.FeeService
{
    public class FeePeriodService : IFeePeriodService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<FeePeriodService> _logger;
        private readonly IStudentService _studentService;

        public FeePeriodService(
            TaekwondoDbContext context,
            ILogger<FeePeriodService> logger,
            IStudentService studentService)
        {
            _context = context;
            _logger = logger;
            _studentService = studentService;
        }
        public async Task<FeePeriodDTO> CreateFeePeriodAsync(FeePeriodRequestDTO feePeriodDTO)
        {
            try
            {
                var feePeriod = new FeePeriod
                {
                    FeePeriodCode = feePeriodDTO.FeePeriodCode,
                    PeriodName = feePeriodDTO.PeriodName,
                    Amount = feePeriodDTO.Amount,
                    StartDate = feePeriodDTO.StartDate,
                    EndDate = feePeriodDTO.EndDate
                };

                _context.FeePeriods.Add(feePeriod);
                await _context.SaveChangesAsync();

                var students = await _studentService.GetAllStudentsAsync();
                foreach (var student in students)
                {
                    if (student.Status != "Active")
                    {
                        continue;
                    }

                    var studentFee = new StudentFee
                    {
                        StudentId = student.StudentId,
                        FeePeriodId = feePeriod.FeePeriodId,
                        Amount = feePeriod.Amount
                    };

                    _context.StudentFees.Add(studentFee);
                }

                await _context.SaveChangesAsync();

                return new FeePeriodDTO
                {
                    FeePeriodId = feePeriod.FeePeriodId,
                    FeePeriodCode = feePeriod.FeePeriodCode,
                    PeriodName = feePeriod.PeriodName,
                    Amount = feePeriod.Amount,
                    StartDate = feePeriod.StartDate,
                    EndDate = feePeriod.EndDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new Fee Period.");
                throw;
            }
        }

        public async Task<bool> DeleteFeePeriodAsync(int feePeriodId)
        {
            try
            {
                var feePeriod = await _context.FeePeriods.FindAsync(feePeriodId);
                if (feePeriod == null)
                {
                    return false;
                }

                _context.FeePeriods.Remove(feePeriod);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting Fee Period with ID {feePeriodId}.");
                throw;
            }
        }

        public async Task<IEnumerable<FeePeriodDTO>> GetAllFeePeriodsAsync()
        {
            try
            {
                var feePeriods = await _context.FeePeriods.ToListAsync();
                return feePeriods.Select(fp => new FeePeriodDTO
                {
                    FeePeriodId = fp.FeePeriodId,
                    FeePeriodCode = fp.FeePeriodCode,
                    PeriodName = fp.PeriodName,
                    Amount = fp.Amount,
                    StartDate = fp.StartDate,
                    EndDate = fp.EndDate,
                    DueDate = fp.DueDate,
                    IsActive = fp.IsActive,
                    CreatedAt = fp.CreatedAt,
                    PublishedAt = fp.PublishedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all Fee Periods.");
                throw;
            }
        }

        public async Task<FeePeriodDTO> GetFeePeriodByIdAsync(int feePeriodId)
        {
            try
            {
                var feePeriod = await _context.FeePeriods.FindAsync(feePeriodId);
                if (feePeriod == null)
                {
                    return null;
                }

                return new FeePeriodDTO
                {
                    FeePeriodId = feePeriod.FeePeriodId,
                    FeePeriodCode = feePeriod.FeePeriodCode,
                    PeriodName = feePeriod.PeriodName,
                    Amount = feePeriod.Amount,
                    StartDate = feePeriod.StartDate,
                    EndDate = feePeriod.EndDate,
                    DueDate = feePeriod.DueDate,
                    IsActive = feePeriod.IsActive,
                    CreatedAt = feePeriod.CreatedAt,
                    PublishedAt = feePeriod.PublishedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Fee Period with ID {feePeriodId}.");
                throw;
            }
        }

        public async Task<FeePeriodDTO> UpdateFeePeriodAsync(int feePeriodId, FeePeriodDTO feePeriodDTO)
        {
            try
            {
                var feePeriod = await _context.FeePeriods.FindAsync(feePeriodId);
                if (feePeriod == null)
                {
                    return null;
                }

                feePeriod.FeePeriodCode = feePeriodDTO.FeePeriodCode;
                feePeriod.PeriodName = feePeriodDTO.PeriodName;
                feePeriod.Amount = feePeriodDTO.Amount;
                feePeriod.StartDate = feePeriodDTO.StartDate;
                feePeriod.EndDate = feePeriodDTO.EndDate;

                _context.FeePeriods.Update(feePeriod);
                await _context.SaveChangesAsync();

                return new FeePeriodDTO
                {
                    FeePeriodId = feePeriod.FeePeriodId,
                    FeePeriodCode = feePeriod.FeePeriodCode,
                    PeriodName = feePeriod.PeriodName,
                    Amount = feePeriod.Amount,
                    StartDate = feePeriod.StartDate,
                    EndDate = feePeriod.EndDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating Fee Period with ID {feePeriodId}.");
                throw;
            }
        }
    }
}