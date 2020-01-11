using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
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

        public Task<HitDaily> CreateAsync(HitDaily hitDaily)
        {
            throw new NotImplementedException();
        }

        public async Task<HitDaily> CreateHitDailyAsync(HitDaily hitDaily)
        {
            await _unitOfWork.HitsDaily.AddAsync(hitDaily);
            await _unitOfWork.CommitAsync();

            return hitDaily;
        }

        public Task DeleteAsync(DateTime priorTo)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteHit(HitDaily hitDaily)
        {
            _unitOfWork.HitsDaily.LogicalDelete(hitDaily);

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<HitDaily>> GetByClientAsync(Guid clientId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<HitDaily> GetByDateAsync(Guid clientId, DateTime date)
        {
            var hitsDaily = await _unitOfWork.HitsDaily.GetByDatesAsync(clientId, date, date);

            foreach (var hitDaily in hitsDaily)
                return hitDaily;

            return null;
        }

        public async Task<IEnumerable<HitDaily>> GetByDatesAsync(Guid clientId, DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.HitsDaily.GetByDatesAsync(clientId, startDate, endDate);
        }

        public async Task<HitDaily> GetByIdAsync(Guid hitId)
        {
            return await _unitOfWork.HitsDaily.GetByIdAsync(hitId);
        }

        public Task<int> GetCountByClientAsync(Guid clientId, string type, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseAsync(HitDaily hitDaily, string type, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseHitDailyAsync(HitDaily hitDaily, string type, decimal price)
        {
            /*var hitDaily2 = await _unitOfWork.HitsDaily.GetByIdAsync(hitDaily.HitDailyId);

            hitDaily2.TotalPrice += price;

            //await _unitOfWork.HitsDaily.
            await _unitOfWork.CommitAsync();*/

            throw new NotImplementedException();
        }
    }
}
