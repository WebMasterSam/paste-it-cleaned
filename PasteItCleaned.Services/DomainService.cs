using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;

namespace PasteItCleaned.Backend.Services
{
    public class DomainService : IDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Domain Create(Domain domain)
        {
            _unitOfWork.Domains.Add(domain);
            _unitOfWork.Commit();

            return domain;
        }

        public void Delete(Guid domainId)
        {
            _unitOfWork.Domains.LogicalDelete(domainId);

            _unitOfWork.Commit();
        }

        public Domain Get(Guid domainId)
        {
            return _unitOfWork.Domains.Get(domainId);
        }

        public Domain GetByName(Guid apiKeyId, string name)
        {
            return _unitOfWork.Domains.GetByName(apiKeyId, name);
        }

        public List<Domain> List(Guid apiKeyId)
        {
            return _unitOfWork.Domains.List(apiKeyId);
        }

        public Domain Update(Domain domain)
        {
            domain.UpdatedOn = DateTime.Now;

            _unitOfWork.Commit();

            return domain;
        }
    }
}
