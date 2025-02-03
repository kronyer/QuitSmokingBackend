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
    }
}

