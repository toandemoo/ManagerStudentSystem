using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetProvider(string providerCode);
    }
}