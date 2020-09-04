using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;
using System.Collections.Generic;

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
                case "powerpoint": hitDaily.CountPowerPoint++; break;
                case "text": hitDaily.CountText++; break;
                case "web": hitDaily.CountWeb++; break;
                case "word": hitDaily.CountWord++; break;
                case "rtf": hitDaily.CountRtf++; break;
                case "openoffice": hitDaily.CountOpenOffice++; break;
                case "libreoffice": hitDaily.CountLibreOffice++; break;
                case "google": hitDaily.CountGoogle++; break;
                case "googlesheets": hitDaily.CountGoogleSheets++; break;
                case "googledocs": hitDaily.CountGoogleDocs++; break;
                case "other": hitDaily.CountOther++; break;
            }

            hitDaily.TotalPrice += price;

            _unitOfWork.Commit();

            return hitDaily;
        }

        public List<HitDaily> List(Guid clientId, DateTime startDate, DateTime endDate)
        {
            return _unitOfWork.HitsDaily.List(clientId, startDate, endDate);
        }
    }
}
