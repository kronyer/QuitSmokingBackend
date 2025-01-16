namespace QuitSmoking.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICigarretesRepository CigarretesRepository { get; }
        ISmokingHistoryRepository SmokingHistoryRepository { get; }
        Task<int> CompleteAsync();
    }
}
