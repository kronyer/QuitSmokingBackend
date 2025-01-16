using QuitSmoking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ICigarretesService
    {
        Task<IEnumerable<Cigarretes>> GetAllAsync();
        Task<Cigarretes> GetByIdAsync(int id);
        Task AddAsync(Cigarretes cigarretes);
        Task UpdateAsync(Cigarretes cigarretes);
        Task DeleteAsync(int id);
    }
}
