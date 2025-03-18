using QuitSmoking.Domain.Entities;

namespace QuitSmoking.Application.Services
{
    public interface ISmokingProgressServices
    {
        Task<bool> CreateFirstChallenge(string userId, int cigarretesPerDay);
        Task<SmokingProgress> GetCurrentChallenge(string userId);
        Task<ChallengeResume> ChallengeResume(string userId);
        Task<bool> IsSuccess(string userId);
    }
}