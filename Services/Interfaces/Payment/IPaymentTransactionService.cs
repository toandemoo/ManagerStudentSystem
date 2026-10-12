using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransaction> CreateAsync(PaymentTransactionRequestDTO request);
        Task<PaymentTransaction?> GetByProviderTransactionIdAsync(int providerId, string providerTransactionId);
        Task MarkSuccessAsync(long transactionId);
        Task MarkFailedAsync(long transactionId);
    }
}