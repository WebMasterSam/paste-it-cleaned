using PasteItCleaned.Backend.Core;
using PasteItCleaned.Backend.Core.Models;
using PasteItCleaned.Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteItCleaned.Backend.Services
{
    public class ErrorService : IErrorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ErrorService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Error> CreateError(Error error)
        {
            await _unitOfWork.Errors.AddAsync(error);
            await _unitOfWork.CommitAsync();

            return error;
        }

        public async Task DeleteError(Error error)
        {
            _unitOfWork.Errors.LogicalDelete(error);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteErrors(DateTime olderThan)
        {
            //_unitOfWork.Errors.LogicalDelete(error);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Error>> GetAllByClientId(Guid clientId)
        {
            return await _unitOfWork.Errors.GetAllByParentIdAsync(clientId);
        }

        public async Task<Error> GetById(Guid errorId)
        {
            return await _unitOfWork.Errors.GetByIdAsync(errorId);
        }
    }
}
