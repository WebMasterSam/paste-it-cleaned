﻿using PasteItCleaned.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<Invoice> GetByNumberAsync(int number);
    }
}