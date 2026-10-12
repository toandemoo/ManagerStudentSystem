using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IWebhookService
    {
        Task HandleAsync(string providerCode, IQueryCollection payload, string signature);
    }
}