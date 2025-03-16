using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingHistoryDomainService
    {
        Task<SmokingScore> GetTodayScoreAsync(string userId);
        Task<IEnumerable<DateTime>> ChartGetSmokedToday(string userId);
        Task<IEnumerable<DateTime>> ChartGetSmokedThisWeek(string userId);
        Task<IEnumerable<DateTime>> ChartGetSmokedThisMonth(string userId);
    }
}
