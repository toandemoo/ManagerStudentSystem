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
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly ILogger<PaymentMethodService> _logger;
        private readonly TaekwondoDbContext _context;

        public PaymentMethodService(ILogger<PaymentMethodService> logger, TaekwondoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<PaymentMethodDTO> CreatePaymentMethodAsync(PaymentMethodDTO paymentMethod)
        {
            try
            {
                var newPaymentMethod = new PaymentMethod
                {
                    PaymentMethodName = paymentMethod.PaymentMethodName,
                    IsActive = paymentMethod.IsActive
                };

                _context.PaymentMethods.Add(newPaymentMethod);
                await _context.SaveChangesAsync();

                return new PaymentMethodDTO
                {
                    PaymentMethodName = newPaymentMethod.PaymentMethodName,
                    IsActive = newPaymentMethod.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new payment method.");
                return null; // or throw an exception, depending on your error handling strategy
            }
        }

        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            try
            {
                var paymentMethod = await _context.PaymentMethods.FindAsync(id);
                if (paymentMethod == null)
                {
                    return false; // Payment method not found
                }

                _context.PaymentMethods.Remove(paymentMethod);
                await _context.SaveChangesAsync();
                return true; // Successfully deleted
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the payment method with ID {id}.");
                return false; // or throw an exception, depending on your error handling strategy
            }
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync()
        {
            try
            {
                var paymentMethods = await _context.PaymentMethods.ToListAsync();
                return paymentMethods.Select(pm => new PaymentMethodDTO
                {
                    PaymentMethodName = pm.PaymentMethodName,
                    IsActive = pm.IsActive
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all payment methods.");
                return Enumerable.Empty<PaymentMethodDTO>(); // or throw an exception, depending on your error handling strategy
            }
        }

        public async Task<PaymentMethodDTO> GetPaymentMethodByIdAsync(int id)
        {
            try
            {
                var paymentMethod = await _context.PaymentMethods.FindAsync(id);
                if (paymentMethod == null)
                {
                    return null; // Payment method not found
                }

                return new PaymentMethodDTO
                {
                    PaymentMethodName = paymentMethod.PaymentMethodName,
                    IsActive = paymentMethod.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the payment method with ID {id}.");
                return null; // or throw an exception, depending on your error handling strategy
            }
        }

        public async Task<PaymentMethodDTO> UpdatePaymentMethodAsync(int id, PaymentMethodDTO paymentMethod)
        {
            try
            {
                var existingPaymentMethod = await _context.PaymentMethods.FindAsync(id);
                if (existingPaymentMethod == null)
                {
                    return null; // Payment method not found
                }

                existingPaymentMethod.PaymentMethodName = paymentMethod.PaymentMethodName;
                existingPaymentMethod.IsActive = paymentMethod.IsActive;

                await _context.SaveChangesAsync();

                return new PaymentMethodDTO
                {
                    PaymentMethodName = existingPaymentMethod.PaymentMethodName,
                    IsActive = existingPaymentMethod.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the payment method with ID {id}.");
                return null; // or throw an exception, depending on your error handling strategy
            }
        }
    }
}