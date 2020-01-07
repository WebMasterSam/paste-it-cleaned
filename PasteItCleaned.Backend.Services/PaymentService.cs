using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CommitAsync();

            return payment;
        }

        public async Task DeletePayment(Payment payment)
        {
            _unitOfWork.Payments.LogicalDelete(payment);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.Payments.GetAllByParentIdAsync(clientId);
        }

        public async Task<Payment> GetById(Guid paymentId)
        {
            return await _unitOfWork.Payments.GetByIdAsync(paymentId);
        }

        public async Task UpdatePayment(Payment paymentToBeUpdated, Payment payment)
        {
            paymentToBeUpdated.TrxNumber = payment.TrxNumber;

            await _unitOfWork.CommitAsync();
        }
    }
}
