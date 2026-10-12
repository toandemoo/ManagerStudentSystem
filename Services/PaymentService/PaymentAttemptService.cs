using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Enums;
using ManagerStudentSystem.Models;
using ManagerStudentSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Services
{
    public class PaymentAttemptService : IPaymentAttemptService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<PaymentAttemptService> _logger;


        public PaymentAttemptService(TaekwondoDbContext context, ILogger<PaymentAttemptService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetNextAttemptNumberAsync(string paymentCode)
        {
            var lastAttempt = await _context.PaymentAttempts
                .Where(x => x.PaymentCode == paymentCode)
                .OrderByDescending(x => x.AttemptNumber)
                .Select(x => (int?)x.AttemptNumber)
                .FirstOrDefaultAsync();

            return (lastAttempt ?? 0) + 1;
        }

        public async Task<PaymentAttempt> CreateAsync(Payment payment, int attemptNumber)
        {
            var attempt = new PaymentAttempt
            {
                PaymentCode = payment.PaymentCode,
                AttemptNumber = attemptNumber,
                Amount = payment.Amount,
                Status = PaymentAttemptStatus.Created.ToString(),
            };

            _context.PaymentAttempts.Add(attempt);

            await _context.SaveChangesAsync();

            return attempt;
        }

        public async Task MarkProcessingAsync(long attemptId)
        {
            var attempt = await GetAttemptAsync(attemptId);

            attempt.Status = PaymentAttemptStatus.Processing.ToString();
            attempt.StartedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task MarkSuccessAsync(long attemptId)
        {
            var attempt = await GetAttemptAsync(attemptId);

            attempt.Status = PaymentAttemptStatus.Success.ToString();
            attempt.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task MarkFailedAsync(
            long attemptId,
            string? errorCode,
            string? errorMessage)
        {
            var attempt = await GetAttemptAsync(attemptId);

            attempt.Status = PaymentAttemptStatus.Failed.ToString();
            attempt.ErrorCode = errorCode;
            attempt.ErrorMessage = errorMessage;
            attempt.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private async Task<PaymentAttempt> GetAttemptAsync(long id)
        {
            var attempt = await _context.PaymentAttempts
                .FirstOrDefaultAsync(x => x.PaymentAttemptId == id);

            if (attempt == null)
                throw new KeyNotFoundException(
                    $"Payment attempt {id} not found.");

            return attempt;
        }

        public Task<PaymentAttempt?> GetLatestAsync(long paymentId)
        {
            throw new NotImplementedException();
        }
    }
}