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
    public class WebhookService : IWebhookService
    {
        private readonly TaekwondoDbContext _context;
        private readonly IEnumerable<IPaymentProvider> _providers;
        private readonly ILogger<WebhookService> _logger;

        public WebhookService(
            TaekwondoDbContext context,
            IEnumerable<IPaymentProvider> providers,
            ILogger<WebhookService> logger)
        {
            _context = context;
            _providers = providers;
            _logger = logger;
        }

        public async Task HandleAsync(
            string providerCode,
            IQueryCollection payload,
            string? signature)
        {
            // 1. Find provider
            var provider = await _context.PaymentProviders
                .FirstOrDefaultAsync(x =>
                    x.PaymentProviderCode == providerCode &&
                    x.IsActive);

            if (provider == null)
                throw new InvalidOperationException(
                    "Payment provider not found.");

            // 2. Find provider implementation
            var paymentProvider = _providers
                .FirstOrDefault(x =>
                    x.Code.Equals(
                        provider.PaymentProviderCode,
                        StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
                throw new InvalidOperationException(
                    "Payment provider implementation not found.");

            // 3. Verify webhook + parse payload
            var webhook = await paymentProvider.ParseWebhookAsync(payload);

            if (!webhook.IsValid)
                throw new InvalidOperationException("Invalid webhook.");

            // 4. Check duplicate webhook
            var existingEvent =
                await _context.WebhookEvents
                    .FirstOrDefaultAsync(x =>
                        x.ProviderId == provider.PaymentProviderId &&
                        x.EventId == webhook.EventId);

            if (existingEvent != null)
            {
                // Already processed / received.
                return;
            }

            // 5. Create WebhookEvent
            var webhookEvent = new WebhookEvent
            {
                ProviderId = provider.PaymentProviderId,
                EventId = webhook.EventId,
                EventType = webhook.EventType,
                ProviderTransactionId = webhook.ProviderTransactionId,
                Payload = payload.ToString(),
                Signature = signature,
                Status = (byte)WebhookStatus.Processing,
                ReceivedAt = DateTime.UtcNow
            };

            _context.WebhookEvents.Add(webhookEvent);

            await _context.SaveChangesAsync();

            // 6. Find payment
            var payment = await _context.Payments
                .Include(x => x.Invoice)
                .Include(x => x.PaymentAttempts)
                .FirstOrDefaultAsync(x =>
                    x.PaymentCode == webhook.PaymentCode);

            if (payment == null)
            {
                webhookEvent.Status = (byte)WebhookStatus.Failed;
                webhookEvent.ErrorMessage = "Payment not found.";

                await _context.SaveChangesAsync();

                throw new KeyNotFoundException("Payment not found.");
            }

            // 7. Validate amount
            if (payment.Amount != webhook.Amount)
            {
                webhookEvent.Status = (byte)WebhookStatus.Failed;
                webhookEvent.ErrorMessage =
                    "Payment amount mismatch.";

                await _context.SaveChangesAsync();

                throw new InvalidOperationException(
                    "Payment amount mismatch.");
            }

            // 8. Find latest attempt
            var attempt = payment.PaymentAttempts
                .OrderByDescending(x => x.AttemptNumber)
                .FirstOrDefault();

            if (attempt == null)
            {
                webhookEvent.Status = (byte)WebhookStatus.Failed;
                webhookEvent.ErrorMessage = "Payment attempt not found.";

                await _context.SaveChangesAsync();

                throw new InvalidOperationException(
                    "Payment attempt not found.");
            }

            // 9. Start DB transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 10. Create transaction
                var paymentTransaction =
                    new PaymentTransaction
                    {
                        PaymentId = payment.PaymentId,
                        PaymentAttemptId = attempt.PaymentAttemptId,
                        ProviderId = provider.PaymentProviderId,
                        ProviderTransactionId =
                            webhook.ProviderTransactionId,
                        TransactionType = (byte)PaymentTransactionType.Sale,
                        Amount = webhook.Amount,
                        Status = webhook.Success
                            ? PaymentTransactionStatus.Succeeded.ToString()
                            : PaymentTransactionStatus.Failed.ToString(),
                        TransactionAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };

                _context.PaymentTransactions.Add(
                    paymentTransaction);

                // 11. Update attempt
                if (webhook.Success)
                {
                    attempt.Status =
                        PaymentAttemptStatus.Success.ToString();

                    attempt.CompletedAt =
                        DateTime.UtcNow;
                }
                else
                {
                    attempt.Status =
                        PaymentAttemptStatus.Failed.ToString();

                    attempt.ErrorCode =
                        webhook.ErrorCode;

                    attempt.ErrorMessage =
                        webhook.ErrorMessage;

                    attempt.CompletedAt =
                        DateTime.UtcNow;
                }

                // 12. Update payment
                if (webhook.Success)
                {
                    payment.Status =
                        PaymentStatus.Succeeded.ToString();

                    payment.PaidAt =
                        DateTime.UtcNow;

                    payment.UpdatedAt =
                        DateTime.UtcNow;
                }
                else
                {
                    payment.Status =
                        PaymentStatus.Failed.ToString();

                    payment.UpdatedAt =
                        DateTime.UtcNow;
                }

                // 13. Update invoice
                if (webhook.Success)
                {
                    var invoice = payment.Invoice;

                    invoice.PaidAmount += payment.Amount;

                    if (invoice.PaidAmount >=
                        invoice.TotalAmount)
                    {
                        invoice.PaidAmount =
                            invoice.TotalAmount;

                        invoice.Status =
                            InvoiceStatus.Paid.ToString();

                        invoice.PaidAt =
                            DateTime.UtcNow;
                    }
                    else
                    {
                        invoice.Status =
                            InvoiceStatus.PartiallyPaid.ToString();
                    }

                    invoice.UpdatedAt =
                        DateTime.UtcNow;
                }

                // 14. Mark webhook processed
                webhookEvent.Status = (byte)WebhookStatus.Processed;

                webhookEvent.ProcessedAt =
                    DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // 15. Commit
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                webhookEvent.Status = (byte)WebhookStatus.Failed;

                webhookEvent.ErrorMessage = "Webhook processing failed.";

                await _context.SaveChangesAsync();

                throw;
            }
        }
    }
}