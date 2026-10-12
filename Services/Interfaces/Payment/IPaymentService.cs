using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.Refund;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> CreatePaymentAsync(PaymentRequestDTO request, string idempotencyKey);
        Task<PaymentProviderResultDTO> ReturnResult(IQueryCollection query);
        Task<PaymentProviderResultDTO> IpnResult(IQueryCollection query);
        Task<RefundResultDTO> RefundAsync(RefundRequestDTO request);
        Task<PaymentResponseDTO> GetByIdAsync(long paymentId);
        Task<PaymentResponseDTO> GetByCodeAsync(string paymentCode);
        Task<PaymentResponseDTO> RetryAsync(string paymentCode);
        Task CancelAsync(string paymentCode);
    }
}