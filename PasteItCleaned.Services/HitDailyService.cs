using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class HitDailyService : IHitDailyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HitDailyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void DeleteByDate(DateTime priorTo)
        {
            _unitOfWork.HitsDaily.DeleteByDate(priorTo);

            _unitOfWork.Commit();
        }

        public HitDaily CreateOrIncrease(Guid clientId, DateTime date, string type, decimal price)
        {
            var hitDaily = _unitOfWork.HitsDaily.Get(clientId, date);

            if (hitDaily == null)
            {
                hitDaily = new HitDaily { ClientId = clientId, Date = date };
                _unitOfWork.HitsDaily.Add(hitDaily);
            }

            switch (type.ToLower())
            {
                case "excel": hitDaily.CountExcel++; break;
                case "image": hitDaily.CountImage++; break;
                case "other": hitDaily.CountOther++; break;
                case "powerpoint": hitDaily.CountPowerPoint++; break;
                case "text": hitDaily.CountText++; break;
                case "web": hitDaily.CountWeb++; break;
                case "word": hitDaily.CountWord++; break;
            }

            hitDaily.TotalPrice += price;

            _unitOfWork.Commit();

            return hitDaily;
        }

        public PagedList<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            return _unitOfWork.HitsDaily.List(clientId, startDate, endDate, page, pageSize);
        }
    }
}
