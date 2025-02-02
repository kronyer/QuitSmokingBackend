using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Services
{
    public class CigarretesService : ICigarretesService
    {
        private readonly ICigarretesRepository _cigarretesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CigarretesService(ICigarretesRepository cigarretesRepository, IUnitOfWork unitOfWork)
        {
            _cigarretesRepository = cigarretesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserCigarrete >> GetAllAsync()
        {
            return await _cigarretesRepository.GetAllAsync();
        }

        public async Task<UserCigarrete> GetByIdAsync(int id)
        {
            return await _cigarretesRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(UserCigarrete cigarretes)
        {
            await _cigarretesRepository.AddAsync(cigarretes);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UserCigarrete cigarretes)
        {
            _cigarretesRepository.UpdateAsync(cigarretes);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _cigarretesRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}




