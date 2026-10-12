using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IRefundService
    {
        // Task<RefundDto> CreateRefundAsync(CreateRefundRequest request);
        // Task<RefundDto> GetByIdAsync(long refundId);
        Task ProcessRefundAsync(long refundId);
    }
}