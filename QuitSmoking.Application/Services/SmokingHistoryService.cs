using Microsoft.AspNetCore.Identity;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

namespace QuitSmoking.Application.Services
{
    public class SmokingHistoryService : ISmokingHistoryService
    {
        private readonly ISmokingHistoryRepository _smokingHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager; // Adicione esta linha


        public SmokingHistoryService(ISmokingHistoryRepository smokingHistoryRepository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) // Modifique o construtor
        {
            _smokingHistoryRepository = smokingHistoryRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager; // Inicialize o _userManager
        }

        public async Task<IEnumerable<SmokingHistory>> GetAllAsync()
        {
            return await _smokingHistoryRepository.GetAllAsync();
        }

        public async Task<SmokingHistory> GetByIdAsync(int id)
        {
            return await _smokingHistoryRepository.GetByIdAsync(id);
        }

        public async Task<SmokingHistory> AddAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                throw new Exception("User not Found");
            } else if (user.CigarreteId is null || user.CigarreteId == 0)
            {
                throw new Exception("User config not set");
            }
            var smokingHistory = new SmokingHistory() { UserId = userId, Date = DateTime.Now, CigarreteId = user.CigarreteId.Value, Quantity = 1 };
            await _unitOfWork.SmokingHistoryRepository.AddAsync(smokingHistory);
            await _unitOfWork.CompleteAsync();
            return smokingHistory;
        }

        public async Task UpdateAsync(SmokingHistory smokingHistory)
        {
           await _unitOfWork.SmokingHistoryRepository.UpdateAsync(smokingHistory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _smokingHistoryRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<DateTime>> GetLastFiveHoursAsync(string userId)
        {
            return await _smokingHistoryRepository.GetLastFiveHoursAsync(userId);
        }

        public async Task<IEnumerable<DateTime>> GetTodaySmoked(string userId)
        {
            return await _smokingHistoryRepository.GetLastFiveHoursAsync(userId);
        }

    }
}




