using QuitSmoking.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingHistoryService
    {
        Task<IEnumerable<SmokingHistory>> GetAllAsync();
        Task<SmokingHistory> GetByIdAsync(int id);
        Task<SmokingHistory> AddAsync(string userId, DateTime? date = null);
        Task UpdateAsync(SmokingHistory smokingHistory);
        Task DeleteAsync(int id);
        Task<IEnumerable<DateTime>> GetLastFiveHoursAsync(string userId);
    }
}




