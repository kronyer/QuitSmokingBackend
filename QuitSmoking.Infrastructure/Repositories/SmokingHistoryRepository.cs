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



    }
}

