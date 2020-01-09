using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class DomainService : IDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Domain> CreateDomain(Domain domain)
        {
            await _unitOfWork.Domains.AddAsync(domain);
            await _unitOfWork.CommitAsync();

            return domain;
        }

        public async Task DeleteDomain(Domain domain)
        {
            _unitOfWork.Domains.LogicalDelete(domain);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Domain>> GetAllByApiKeyId(Guid apiKeyId)
        {
            return await _unitOfWork.Domains.GetAllByParentIdAsync(apiKeyId);
        }

        public async Task<Domain> GetById(Guid configId)
        {
            return await _unitOfWork.Domains.GetByIdAsync(configId);
        }

        public async Task<Domain> GetByName(string name)
        {
            return await _unitOfWork.Domains.GetByNameAsync(name);
        }

        public async Task UpdateConfig(Domain domainToBeUpdated, Domain domain)
        {
            domainToBeUpdated.Name = domain.Name;
            domainToBeUpdated.UpdatedOn = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }

        public Task UpdateDomain(Domain domainToBeUpdated, Domain domain)
        {
            throw new NotImplementedException();
        }
    }
}
