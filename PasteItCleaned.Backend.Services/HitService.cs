using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class HitService : IHitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HitService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Hit> CreateHit(Hit hit)
        {
            await _unitOfWork.Hits.AddAsync(hit);
            await _unitOfWork.CommitAsync();

            return hit;
        }

        public async Task DeleteHit(Hit hit)
        {
            _unitOfWork.Hits.LogicalDelete(hit);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Hit>> GetAllByClientIdAsync(Guid clientId)
        {
            return await _unitOfWork.Hits.GetAllByParentIdAsync(clientId);
        }

        public async Task<Hit> GetByHashAsync(int hash)
        {
            return await _unitOfWork.Hits.GetByHashAsync(hash);
        }

        public async Task<Hit> GetByIdAsync(Guid HitId)
        {
            return await _unitOfWork.Hits.GetByIdAsync(HitId);
        }
    }
}
