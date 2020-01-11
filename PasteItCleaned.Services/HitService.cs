using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class HitService : IHitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HitService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Hit Create(Hit hit)
        {
            _unitOfWork.Hits.Add(hit);
            _unitOfWork.Commit();

            return hit;
        }

        public void DeleteByDate(DateTime priorTo)
        {
            _unitOfWork.Hits.DeleteByDate(priorTo);

            _unitOfWork.Commit();
        }

        public Hit GetByHash(Guid clientId, DateTime date, int hash)
        {
            return _unitOfWork.Hits.GetByHash(clientId, date, hash);
        }

        public PagedList<Hit> List(Guid clientId, string type, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            return _unitOfWork.Hits.List(clientId, type, startDate, endDate, page, pageSize);
        }
    }
}
