using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Repositories;
using PasteItCleaned.Backend.Data.Repositories;

namespace PasteItCleaned.Backend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PasteItCleanedDbContext _context;
        private ApiKeyRepository _apiKeyRepository;
        private ClientRepository _clientRepository;
        private ConfigRepository _configRepository;
        private DomainRepository _domainRepository;
        private ErrorRepository _errorRepository;
        private HitDailyRepository _hitDailyRepository;
        private HitRepository _hitRepository;
        private InvoiceRepository _invoiceRepository;
        private PaymentMethodRepository _paymentMethodRepository;
        private PaymentRepository _paymentRepository;
        private UserRepository _userRepository;

        public UnitOfWork(PasteItCleanedDbContext context)
        {
            this._context = context;
        }

        public IApiKeyRepository ApiKeys => _apiKeyRepository = _apiKeyRepository ?? new ApiKeyRepository(_context);

        public IClientRepository Clients => _clientRepository = _clientRepository ?? new ClientRepository(_context);

        public IConfigRepository Configs => _configRepository = _configRepository ?? new ConfigRepository(_context);

        public IDomainRepository Domains => _domainRepository = _domainRepository ?? new DomainRepository(_context);

        public IErrorRepository Errors => _errorRepository = _errorRepository ?? new ErrorRepository(_context);

        public IHitDailyRepository HitsDaily => _hitDailyRepository = _hitDailyRepository ?? new HitDailyRepository(_context);

        public IHitRepository Hits => _hitRepository = _hitRepository ?? new HitRepository(_context);

        public IInvoiceRepository Invoices => _invoiceRepository = _invoiceRepository ?? new InvoiceRepository(_context);

        public IPaymentMethodRepository PaymentMethods => _paymentMethodRepository = _paymentMethodRepository ?? new PaymentMethodRepository(_context);

        public IPaymentRepository Payments => _paymentRepository = _paymentRepository ?? new PaymentRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
