using GreenLoop.DAL.Entities;

namespace GreenLoop.DAL.Interfaces.IRepositories
{
    public interface IPointTransactionRepository
    {
        Task AddAsync(PointTransaction transaction);
        Task<PointTransaction?> GetLatestTransactionAsync(int customerId, int driverId);
        Task<bool> HasScannedRecentlyAsync(int customerId, string qrCodeValue, int minutes);
    }
}
