using PasteItCleaned.Backend.Core.Repositories;
using System;

namespace PasteItCleaned.Backend.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IApiKeyRepository ApiKeys { get; }
        IClientRepository Clients { get; }
        IConfigRepository Configs { get; }
        IDomainRepository Domains { get; }
        IErrorRepository Errors { get; }
        IHitDailyRepository HitsDaily { get; }
        IHitRepository Hits { get; }
        IInvoiceRepository Invoices { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        IPaymentRepository Payments { get; }
        IUserRepository Users { get; }
        int Commit();
    }
}
