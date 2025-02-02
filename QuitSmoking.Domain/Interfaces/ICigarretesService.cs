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
        Task<IEnumerable<UserCigarrete>> GetAllAsync();
        Task<UserCigarrete> GetByIdAsync(int id);
        Task AddAsync(UserCigarrete cigarretes);
        Task UpdateAsync(UserCigarrete cigarretes);
        Task DeleteAsync(int id);
    }
}
