using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

namespace QuitSmoking.Infrastructure.Repositories
{
    public class CigarretesRepository : Repository<Cigarretes>, ICigarretesRepository
    {
        public CigarretesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
