using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Payment Create(Payment payment)
        {
            _unitOfWork.Payments.Add(payment);
            _unitOfWork.Commit();

            return payment;
        }

        public Payment Get(Guid paymentId)
        {
            return _unitOfWork.Payments.Get(paymentId);
        }

        public Payment GetByInvoice(Guid invoiceId)
        {
            return _unitOfWork.Payments.GetByInvoice(invoiceId);
        }

        public Payment Update(Payment payment)
        {
            _unitOfWork.Commit();

            return payment;
        }
    }
}
