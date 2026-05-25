namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IBookingRepo
    {
        Task AddAsync(Booking booking, CancellationToken cancellation = default);
        IQueryable<Booking> GetAll();
        IQueryable<Booking> GetById(int id);
        IQueryable<Booking> GetByPlayerId(int playerId);
        IQueryable<Booking> GetByFieldOwnerId(int fieldOwnerId);
        IQueryable<Booking> GetByFieldSlotId(int fieldSlotId);
        Task UpdateStatusAsync(int id, BookingStatus status, CancellationToken cancellation = default);
        Task DeleteAsync(int id, CancellationToken cancellation = default);
        Task<bool> IsExistAsync(int id, CancellationToken cancellation = default);
    }
}
