using PasteItCleaned.Backend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core.Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAllByClientId(Guid clientId);
        Task<PaymentMethod> GetById(Guid paymentMethodId);
        Task<PaymentMethod> CreateApiKey(PaymentMethod paymentMethod);
        Task UpdatePaymentMethod(PaymentMethod paymentMethodToBeUpdated, PaymentMethod paymentMethod);
        Task DeletePaymentMethod(PaymentMethod paymentMethod);
    }
}
