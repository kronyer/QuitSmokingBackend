using Microsoft.EntityFrameworkCore;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

namespace QuitSmoking.Infrastructure.Repositories
{
    public class SmokingHistoryRepository : Repository<SmokingHistory>, ISmokingHistoryRepository
    {
        private readonly DbSet<SmokingHistory> _smokingHistoryDbSet;

        public SmokingHistoryRepository(ApplicationDbContext context) : base(context)
        {
            _smokingHistoryDbSet = context.Set<SmokingHistory>();
        }

        public async Task<IEnumerable<DateTime>> GetLastFiveHoursAsync(string userId)
        {
            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date >= DateTime.Now.AddHours(-4))
                .Select(sh => sh.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<SmokingHistory>> GetSmokedBeforeToday(string userId)
        {
            var lastDate = await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date < DateTime.Today)
                .OrderByDescending(sh => sh.Date)
                .Select(sh => sh.Date.Date)
                .FirstOrDefaultAsync();

            if (lastDate == default)
            {
                return new List<SmokingHistory>();
            }

            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date == lastDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SmokingHistory>> GetTodaySmokedAsync(string userId)
        {
            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date == DateTime.Today)
                .ToListAsync();
        }

        public async Task<int> GetSmokedThisWeek(string userId, DateTime startDate)
        {
            var endDate = startDate.AddDays(7);
            var smokedThisWeek = await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date >= startDate.Date && sh.Date.Date <= endDate.Date)
                .CountAsync();

            return smokedThisWeek;
        }

        public async Task<int> HowManySmokedSince(string userId, DateTime startDate)
        {
            var smokedSince = await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date >= startDate.Date)
                .CountAsync();
            return smokedSince;
        }

        public async Task<IEnumerable<DateTime>> ChartGetSmokedThisMonth(string userId)
        {
            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Month == DateTime.Now.Month)
                .Select(sh => sh.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> ChartGetSmokedThisWeek(string userId)
        {
            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date >= DateTime.Now.AddDays(-7).Date)
                .Select(sh => sh.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> ChartGetSmokedToday(string userId)
        {
            return await _smokingHistoryDbSet
                .Where(sh => sh.UserId == userId && sh.Date.Date == DateTime.Today)
                .Select(sh => sh.Date)
                .ToListAsync();
        }

    }
}

