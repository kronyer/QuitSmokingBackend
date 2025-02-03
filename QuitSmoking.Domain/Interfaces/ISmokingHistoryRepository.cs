using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingHistoryRepository : IRepository<SmokingHistory>
    {
        Task<IEnumerable<DateTime>> GetLastFiveHoursAsync(string userId);


    }
}
