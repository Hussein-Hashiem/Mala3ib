namespace Mala3ib.DAL.Repo.Implementation
{
    public class BookingRepo : IBookingRepo
    {
        private readonly ApplicationDbContext _context;

        public BookingRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellation = default)
        {
            await _context.Bookings.AddAsync(booking, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public IQueryable<Booking> GetAll()
        {
            var bookings = _context.Bookings
                .Where(x => !x.IsDeleted)
                .AsNoTracking();

            return bookings;
        }

        public IQueryable<Booking> GetById(int id)
        {
            var booking = _context.Bookings
                .Where(x => x.Id == id && !x.IsDeleted)
                .AsNoTracking();

            return booking;
        }

        public IQueryable<Booking> GetByPlayerId(int playerId)
        {
            var bookings = _context.Bookings
                .Where(x => x.PlayerId == playerId && !x.IsDeleted)
                .AsNoTracking();

            return bookings;
        }

        public IQueryable<Booking> GetByFieldOwnerId(int fieldOwnerId)
        {
            var bookings = _context.Bookings
                .Where(x => !x.IsDeleted && x.FieldSlot.Field.FieldOwnerId == fieldOwnerId)
                .AsNoTracking();

            return bookings;
        }

        public IQueryable<Booking> GetByFieldSlotId(int fieldSlotId)
        {
            var bookings = _context.Bookings
                .Where(x => x.FieldSlotId == fieldSlotId && !x.IsDeleted)
                .AsNoTracking();

            return bookings;
        }

        public async Task UpdateStatusAsync(int id, BookingStatus status, CancellationToken cancellation = default)
        {
            await _context.Bookings
                .Where(x => x.Id == id && !x.IsDeleted)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.Status, status), cancellation
                );
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await _context.Bookings
                .Where(x => x.Id == id && !x.IsDeleted)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.IsDeleted, true), cancellation
                );
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellation = default)
        {
            return await _context.Bookings
                .AnyAsync(x => x.Id == id && !x.IsDeleted, cancellation);
        }
    }
}
