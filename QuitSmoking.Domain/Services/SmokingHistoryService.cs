using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Services
{
    public class SmokingHistoryService : ISmokingHistoryService
    {
        private readonly ISmokingHistoryRepository _smokingHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SmokingHistoryService(ISmokingHistoryRepository smokingHistoryRepository, IUnitOfWork unitOfWork)
        {
            _smokingHistoryRepository = smokingHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SmokingHistory>> GetAllAsync()
        {
            return await _smokingHistoryRepository.GetAllAsync();
        }

        public async Task<SmokingHistory> GetByIdAsync(int id)
        {
            return await _smokingHistoryRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(SmokingHistory smokingHistory)
        {
            smokingHistory.Date = DateTime.Now;
            await _smokingHistoryRepository.AddAsync(smokingHistory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(SmokingHistory smokingHistory)
        {
            _smokingHistoryRepository.UpdateAsync(smokingHistory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _smokingHistoryRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}




