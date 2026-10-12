using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Dtos.PaymentProvider
{
    public class PaymentQueryRequestDTO
    {
        public string ProviderTransactionId { get; set; }
        public string TransactionDate { get; set; }

    }
}