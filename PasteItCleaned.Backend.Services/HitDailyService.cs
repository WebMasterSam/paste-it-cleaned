using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class HitDailyService : IHitDailyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HitDailyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<HitDaily> CreateHitDaily(HitDaily hitDaily)
        {
            await _unitOfWork.HitsDaily.AddAsync(hitDaily);
            await _unitOfWork.CommitAsync();

            return hitDaily;
        }

        public async Task DeleteHit(HitDaily hitDaily)
        {
            _unitOfWork.HitsDaily.LogicalDelete(hitDaily);

            await _unitOfWork.CommitAsync();
        }

        public Task<HitDaily> Get(Guid clientId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HitDaily>> GetAllByClientIdAsync(Guid clientId)
        {
            return await _unitOfWork.HitsDaily.GetAllByParentIdAsync(clientId);
        }

        public async Task<HitDaily> GetByIdAsync(Guid hitId)
        {
            return await _unitOfWork.HitsDaily.GetByIdAsync(hitId);
        }

        public Task IncreaseHitDaily(HitDaily hitDaily, string type, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
