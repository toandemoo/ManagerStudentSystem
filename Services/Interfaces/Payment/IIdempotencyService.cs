using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerStudentSystem.Dtos;
using ManagerStudentSystem.Models;

namespace ManagerStudentSystem.Services.Interfaces
{
    public interface IIdempotencyService
    {

        Task<IdempotencyKey?> GetAsync(string key, string endpoint);
        // Task<IdempotencyResult?> GetAsync(string key, string endpoint);

        Task SaveAsync(string key, string endpoint, IdempotencyResult result);
    }
}