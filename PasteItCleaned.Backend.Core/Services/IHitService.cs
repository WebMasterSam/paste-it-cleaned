﻿using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IHitService
    {
        Task<IEnumerable<Hit>> GetAllByClientIdAsync(Guid clientId);
        Task<Hit> GetByIdAsync(Guid hitDailyId);
        Task<Hit> GetByHashAsync(int hash);
        Task<Hit> CreateHit(Hit hit);
    }
}
