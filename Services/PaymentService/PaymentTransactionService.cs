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
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<PaymentTransactionService> _logger;

        public PaymentTransactionService(
            TaekwondoDbContext context,
            ILogger<PaymentTransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaymentTransaction> CreateAsync(PaymentTransactionRequestDTO request)
        {
            var existing = await GetByProviderTransactionIdAsync(
                request.ProviderId,
                request.ProviderTransactionId);

            if (existing != null)
                return existing;

            var transaction = new PaymentTransaction
            {
                PaymentId = request.PaymentId,
                PaymentAttemptId = request.PaymentAttemptId,
                ProviderId = request.ProviderId,
                ProviderTransactionId = request.ProviderTransactionId,
                TransactionType = (byte)request.Type,
                Amount = request.Amount,
                Status = request.Status,
                TransactionAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.PaymentTransactions.Add(transaction);

            await _context.SaveChangesAsync();

            return transaction;
        }


        public async Task<PaymentTransaction?>
            GetByProviderTransactionIdAsync(
                int providerId,
                string providerTransactionId)
        {
            return await _context.PaymentTransactions
                .FirstOrDefaultAsync(x =>
                    x.ProviderId == providerId &&
                    x.ProviderTransactionId ==
                        providerTransactionId);
        }

        public Task MarkFailedAsync(long transactionId)
        {
            throw new NotImplementedException();
        }

        public Task MarkSuccessAsync(long transactionId)
        {
            throw new NotImplementedException();
        }
    }
}