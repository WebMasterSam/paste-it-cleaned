using PasteItCleaned.Core.Models;
using PasteItCleaned.Backend.Core.Repositories;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(PasteItCleanedDbContext context) : base(context)
        { }
    }
}
