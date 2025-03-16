using Microsoft.EntityFrameworkCore;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Infrastructure.Repositories
{
    public class SmokingProgressRepository : Repository<SmokingProgress>, ISmokingProgressRepository
    {
        private readonly DbSet<SmokingProgress> _context;

        public SmokingProgressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context.Set<SmokingProgress>();
        }


        public async Task<SmokingProgress> GetCurrentChallenge(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            return await _context.Where(x => x.UserId == userId)
                                 .OrderByDescending(x => x.LastUpdated)
                                 .FirstOrDefaultAsync();
        }

        public async Task<SmokingProgress> GetFirstChallenge(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            return await _context.Where(x => x.UserId == userId)
                                 .OrderBy(x => x.CreatedAt)
                                 .FirstOrDefaultAsync();
        }
    }
}
