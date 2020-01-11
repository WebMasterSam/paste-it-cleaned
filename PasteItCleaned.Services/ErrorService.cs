using PasteItCleaned.Backend.Core;
using PasteItCleaned.Core.Models;
using PasteItCleaned.Core.Services;
using System;

namespace PasteItCleaned.Backend.Services
{
    public class ErrorService : IErrorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ErrorService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Error Create(Error error)
        {
            _unitOfWork.Errors.Add(error);
            _unitOfWork.Commit();

            return error;
        }

        public void Delete(DateTime priorTo)
        {
            _unitOfWork.Errors.DeleteByDate(priorTo);

            _unitOfWork.Commit();
        }

        public PagedList<Error> List(Guid clientId, int page, int pageSize)
        {
            return _unitOfWork.Errors.List(clientId, page, pageSize);
        }
    }
}
