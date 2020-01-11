using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Linq;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(PasteItCleanedDbContext context) : base(context)
        { }

        public PaymentMethod GetCurrent(Guid clientId)
        {
            return Context.PaymentMethods
                .Where(m => m.ClientId == clientId)
                .Where(m => !m.Deleted)
                .FirstOrDefault();
        }

        public void LogicalDelete(Guid paymentMethodId)
        {
            var entity = Context.PaymentMethods
                .Where(m => m.PaymentMethodId == paymentMethodId)
                .FirstOrDefault();

            entity.Deleted = true;
            entity.UpdatedOn = DateTime.UtcNow;
        }
    }
}
