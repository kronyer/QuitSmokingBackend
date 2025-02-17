using Microsoft.EntityFrameworkCore;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;

namespace QuitSmoking.Infrastructure.Repositories
{
    public class CigarretesRepository : Repository<UserCigarrete>, ICigarretesRepository
    {
        private readonly DbSet<UserCigarrete> _context;

        public CigarretesRepository(ApplicationDbContext context) : base(context)
        {
        }



    }
}
