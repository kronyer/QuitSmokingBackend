using QuitSmoking.Domain.Interfaces;
using QuitSmoking.Infrastructure.Repositories;

namespace QuitSmoking.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICigarretesRepository _cigarretesRepository;
        private ISmokingHistoryRepository _smokingHistoryRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICigarretesRepository CigarretesRepository =>
            _cigarretesRepository ??= new CigarretesRepository(_context);

        public ISmokingHistoryRepository SmokingHistoryRepository =>
            _smokingHistoryRepository ??= new SmokingHistoryRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
