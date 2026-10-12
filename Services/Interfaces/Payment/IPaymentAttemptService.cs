using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentAttemptService
    {
        Task<PaymentAttempt> CreateAsync(Payment payment, int attemptNumber);
        Task MarkProcessingAsync(long attemptId);
        Task MarkSuccessAsync(long attemptId);
        Task MarkFailedAsync(long attemptId, string errorCode, string errorMessage);
        Task<PaymentAttempt?> GetLatestAsync(long paymentId);
        Task<int> GetNextAttemptNumberAsync(string paymentCode);
    }
}