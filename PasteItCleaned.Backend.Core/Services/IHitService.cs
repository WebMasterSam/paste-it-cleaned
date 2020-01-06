using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IHitService
    {
        Task<IEnumerable<Hit>> GetAllByClientId(Guid clientId);
        Task<Hit> GetById(Guid hitDailyId);
        Task<Hit> GetByHash(string hash);
        Task<Hit> CreateHit(Hit hit);
    }
}
