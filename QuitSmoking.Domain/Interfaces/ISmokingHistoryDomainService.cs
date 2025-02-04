using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingHistoryDomainService
    {
        Task<SmokingScore> GetTodayScoreAsync(string userId);
    }
}
