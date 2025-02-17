using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingHistoryRepository : IRepository<SmokingHistory>
    {
        Task<IEnumerable<DateTime>> GetLastFiveHoursAsync(string userId);
        Task<IEnumerable<SmokingHistory>> GetTodaySmokedAsync(string userId);
        Task<IEnumerable<SmokingHistory>> GetSmokedBeforeToday(string userId);
        Task<int> GetSmokedThisWeek(string userId, DateTime startDate);


    }
}
