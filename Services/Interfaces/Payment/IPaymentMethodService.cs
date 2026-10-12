using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync();
        Task<PaymentMethodDTO> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodDTO> CreatePaymentMethodAsync(PaymentMethodDTO paymentMethod);
        Task<PaymentMethodDTO> UpdatePaymentMethodAsync(int id, PaymentMethodDTO paymentMethod);
        Task<bool> DeletePaymentMethodAsync(int id);
    }
}