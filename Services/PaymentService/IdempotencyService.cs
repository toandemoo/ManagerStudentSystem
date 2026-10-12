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
    public class IdempotencyService : IIdempotencyService
    {
        private readonly TaekwondoDbContext _context;
        private readonly ILogger<IdempotencyService> _logger;

        public IdempotencyService(
            TaekwondoDbContext context,
            ILogger<IdempotencyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IdempotencyKey?> GetAsync(string key, string endpoint)
        {
            return await _context.IdempotencyKeys
                .FirstOrDefaultAsync(x =>
                    x.KeyValue == key &&
                    x.Endpoint == endpoint &&
                    x.ExpireAt > DateTime.UtcNow);
        }

        public async Task SaveAsync(
            string key,
            string endpoint,
           IdempotencyResult result)
        {
            var entity = new IdempotencyKey
            {
                KeyValue = key,
                Endpoint = endpoint,
                RequestHash = result.RequestHash,
                ResponseStatusCode = result.ResponseStatus,
                ResponseBody = result.ResponseBody,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.Add(result.Expiration)
            };

            _context.IdempotencyKeys.Add(entity);

            await _context.SaveChangesAsync();
        }
    }
}