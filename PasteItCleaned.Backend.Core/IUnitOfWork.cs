using PasteItCleaned.Backend.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IApiKeyRepository ApiKeys { get; }
        IClientRepository Clients { get; }
        IConfigRepository Configs { get; }
        IErrorRepository Errors { get; }
        IHitDailyRepository HitsDaily { get; }
        IHitRepository Hits { get; }
        IInvoiceRepository Invoices { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        IPaymentRepository Payments { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
