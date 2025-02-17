using Microsoft.AspNetCore.Identity;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System.Threading.Tasks;

namespace QuitSmoking.Application.Services
{
    public class SmokingProgressService : ISmokingProgressServices
    {
        private readonly ISmokingProgressRepository _smokingProgressRepository;
        public readonly ISmokingHistoryService _smokingHistoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SmokingProgressService(ISmokingProgressRepository smokingProgressRepository, UserManager<ApplicationUser> userManager, ISmokingHistoryService smokingHistoryService)
        {
            _smokingProgressRepository = smokingProgressRepository;
            _smokingHistoryService = smokingHistoryService;
            _userManager = userManager;
        }

        public async Task<bool> IsSuccess(string userId)
        {
            //this should happen only if it's passed 7 
            var test = await GetCurrentChallenge(userId);
            if (test.LastUpdated.AddDays(7) > DateTime.Now)
                return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var smokedThisWeek = await _smokingHistoryService.GetSmokedThisWeek(userId, test.LastUpdated);
            var success =   smokedThisWeek < test.NewGoal * 7;

            if (success is true)
            {
                test.Success = true;
                SmokingProgress newProgress = new() { CreatedAt = DateTime.Now, LastUpdated = DateTime.Now, UserId = userId, Success = false, NewGoal = test.NewGoal - 1 };
                 await _smokingProgressRepository.AddAsync(newProgress);
            }
            else
            {
                test.LastUpdated = DateTime.Now;
                await _smokingProgressRepository.UpdateAsync(test);
            }
            return success;
        }

        public async Task<SmokingProgress> GetCurrentChallenge(string userId)
        {
            return await _smokingProgressRepository.GetCurrentChallenge(userId);
        }
    }
}

