using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

namespace QuitSmoking.Infrastructure.Repositories
{
    public class SmokingHistoryRepository : Repository<SmokingHistory>, ISmokingHistoryRepository
    {
        public SmokingHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
