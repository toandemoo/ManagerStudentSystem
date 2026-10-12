using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Dtos.PaymentProvider;
using ManagerStudentSystem.Dtos.Refund;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentProvider
    {
        string Code { get; }
        Task<PaymentProviderResultDTO> CreatePaymentAsync(PaymentProviderRequestDTO request);
        Task<PaymentProviderResultDTO> QueryPaymentAsync(PaymentQueryRequestDTO request);
        Task<PaymentProviderResultDTO> ProcessReturnAsync(IQueryCollection query);
        Task<PaymentProviderResultDTO> ProcessIPNAsync(IQueryCollection query);
        Task<RefundResultDTO> RefundAsync(RefundRequestDTO request);
        bool VerifyWebhook(string payload, string signature);
        Task<WebhookResultDTO> ParseWebhookAsync(IQueryCollection payload);
    }
}