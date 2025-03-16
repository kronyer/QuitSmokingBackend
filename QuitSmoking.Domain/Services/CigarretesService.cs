using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

// QuitSmoking.Domain.Services
namespace QuitSmoking.Domain.Services
{
    public class SmokingHistoryDomainService : ISmokingHistoryDomainService
    {
        private readonly ISmokingHistoryRepository _smokingHistoryRepository;

        public SmokingHistoryDomainService(ISmokingHistoryRepository smokingHistoryRepository)
        {
            _smokingHistoryRepository = smokingHistoryRepository;
        }

        public Task<IEnumerable<DateTime>> ChartGetSmokedThisMonth(string userId)
        {
            return _smokingHistoryRepository.ChartGetSmokedThisMonth(userId);
        }

        public Task<IEnumerable<DateTime>> ChartGetSmokedThisWeek(string userId)
        {
            return _smokingHistoryRepository.ChartGetSmokedThisWeek(userId);
        }

        public async Task<IEnumerable<DateTime>> ChartGetSmokedToday(string userId)
        {
            var result = await _smokingHistoryRepository.ChartGetSmokedToday(userId);
            return result;
        }

        public async Task<SmokingScore> GetTodayScoreAsync(string userId)
        {
            var todaySmoked = await _smokingHistoryRepository.GetTodaySmokedAsync(userId);
            var lastSmoked = await _smokingHistoryRepository.GetSmokedBeforeToday(userId);

            var lastSmokedDate = lastSmoked.FirstOrDefault()?.Date;


            // CigarretesScore é uma entidade de domínio, não um DTO
            return new SmokingScore
            {
                SmokedToday = todaySmoked.Count(),
                SmokedBefore = lastSmoked.Count(),
                Date = DateTime.Today,
                LastSmokedDate = lastSmokedDate ?? DateTime.Today
            };
        }
    }
}





