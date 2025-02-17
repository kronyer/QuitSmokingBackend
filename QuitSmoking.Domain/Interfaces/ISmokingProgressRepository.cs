using QuitSmoking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Interfaces
{
    public interface ISmokingProgressRepository : IRepository<SmokingProgress>
    {
        Task<SmokingProgress> GetCurrentChallenge(string userId);
    }
}
