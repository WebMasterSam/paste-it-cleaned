using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllByClientId(Guid clientId);
        Task<Payment> GetById(Guid paymentId);
        Task<Payment> CreatePayment(Payment payment);
        Task UpdatePayment(Payment paymentToBeUpdated, Payment payment);
        Task DeletePayment(Payment payment);
    }
}
