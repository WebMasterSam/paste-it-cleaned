using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public PaymentMethod Create(PaymentMethod paymentMethod)
        {
            _unitOfWork.PaymentMethods.Add(paymentMethod);
            _unitOfWork.Commit();

            return paymentMethod;
        }

        public void Delete(Guid paymentMethodId)
        {
            _unitOfWork.PaymentMethods.LogicalDelete(paymentMethodId);

            _unitOfWork.Commit();
        }

        public void DeleteCurrent(Guid clientId)
        {
            var paymentMethod = _unitOfWork.PaymentMethods.GetCurrent(clientId);

            _unitOfWork.PaymentMethods.LogicalDelete(paymentMethod.PaymentMethodId);

            _unitOfWork.Commit();
        }

        public PaymentMethod GetCurrent(Guid clientId)
        {
            return _unitOfWork.PaymentMethods.GetCurrent(clientId);
        }
    }
}
