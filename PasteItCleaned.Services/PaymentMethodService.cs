using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<PaymentMethod> CreatePaymentMethod(PaymentMethod paymentMethod)
        {
            await _unitOfWork.PaymentMethods.AddAsync(paymentMethod);
            await _unitOfWork.CommitAsync();

            return paymentMethod;
        }

        public async Task DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            _unitOfWork.PaymentMethods.LogicalDelete(paymentMethod);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.PaymentMethods.GetAllByParentIdAsync(clientId);
        }

        public async Task<PaymentMethod> GetById(Guid paymentMethodId)
        {
            return await _unitOfWork.PaymentMethods.GetByIdAsync(paymentMethodId);
        }

        public async Task UpdatePaymentMethod(PaymentMethod paymentMethodToBeUpdated, PaymentMethod paymentMethod)
        {
            paymentMethodToBeUpdated.UpdatedOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }
    }
}
