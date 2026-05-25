using Mala3ib.BLL.Contracts.Booking;

namespace Mala3ib.BLL.Service.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IPlayerRepo _playerRepo;
        private readonly IFieldSlotRepo _fieldSlotRepo;
        private readonly IInvitationRepo _invitationRepo;

        public BookingService(IBookingRepo bookingRepo, IPlayerRepo playerRepo, IFieldSlotRepo fieldSlotRepo, IInvitationRepo invitationRepo)
        {
            _bookingRepo = bookingRepo;
            _playerRepo = playerRepo;
            _fieldSlotRepo = fieldSlotRepo;
            _invitationRepo = invitationRepo;
        }

        public async Task<Result> AddAsync(string currentUserId, int fieldSlotId, CancellationToken cancellation = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(currentUserId, cancellation);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure(PlayerErrors.NotFound);

            var fieldSlot = await _fieldSlotRepo.GetById(fieldSlotId)
                .FirstOrDefaultAsync(cancellation);

            if (fieldSlot is null)
                return Result.Failure(FieldSlotErrors.NotFound);

            if (fieldSlot.IsBooked)
                return Result.Failure(BookingErrors.SlotUnavailable);

            if (fieldSlot.Players.Any())
                return Result.Failure(BookingErrors.SlotUnavailable);

            var alreadyInSlot = await _fieldSlotRepo.IsPlayerInSlotAsync(fieldSlotId, (int)playerId, cancellation);
            if (alreadyInSlot)
                return Result.Failure(BookingErrors.AlreadyJoined);

            var booking = new Booking
            {
                Date = DateTime.UtcNow,
                FieldSlotId = fieldSlotId,
                PlayerId = (int)playerId,
                Status = BookingStatus.Pending
            };

            await _bookingRepo.AddAsync(booking, cancellation);
            await _fieldSlotRepo.AddPlayerToSlotAsync(fieldSlotId, (int)playerId, true, cancellation);
            await _fieldSlotRepo.UpdateBookedStatusAsync(fieldSlotId, true, cancellation);

            return Result.Success();
        }

        public async Task<Result> CancelAsync(string currentUserId, int bookingId, CancellationToken cancellation = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(currentUserId, cancellation);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure(PlayerErrors.NotFound);

            var booking = await _bookingRepo.GetById(bookingId)
                .FirstOrDefaultAsync(cancellation);

            if (booking is null)
                return Result.Failure(BookingErrors.NotFound);

            if (booking.PlayerId != playerId)
                return Result.Failure(BookingErrors.Unauthorized);

            await _fieldSlotRepo.ClearPlayersFromSlotAsync(booking.FieldSlotId, cancellation);
            await _invitationRepo.DeletePendingByFieldSlotIdAsync(booking.FieldSlotId, cancellation);
            await _fieldSlotRepo.UpdateBookedStatusAsync(booking.FieldSlotId, false, cancellation);
            await _bookingRepo.UpdateStatusAsync(bookingId, BookingStatus.Cancelled, cancellation);
            await _bookingRepo.DeleteAsync(bookingId, cancellation);

            return Result.Success();
        }

        public async Task<Result<BookingResponseDto>> GetByIdAsync(string currentUserId, int bookingId, CancellationToken cancellation = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(currentUserId, cancellation);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure<BookingResponseDto>(PlayerErrors.NotFound);

            var booking = await _bookingRepo.GetById(bookingId)
                .Select(x => new BookingResponseDto(
                    x.Id,
                    x.Date,
                    x.FieldSlotId,
                    x.PlayerId,
                    x.Status
                ))
                .FirstOrDefaultAsync(cancellation);

            if (booking is null)
                return Result.Failure<BookingResponseDto>(BookingErrors.NotFound);

            if (booking.PlayerId != playerId)
                return Result.Failure<BookingResponseDto>(BookingErrors.Unauthorized);

            return Result.Success(booking);
        }

        public async Task<Result<IEnumerable<BookingResponseDto>>> GetMyBookingsAsync(string currentUserId, CancellationToken cancellation = default)
        {
            var playerId = await _playerRepo.GetPlayerIdByUserIdAsync(currentUserId, cancellation);

            if (playerId is null || !playerId.HasValue)
                return Result.Failure<IEnumerable<BookingResponseDto>>(PlayerErrors.NotFound);

            var bookings = await _bookingRepo.GetByPlayerId((int)playerId)
                .Select(x => new BookingResponseDto(
                    x.Id,
                    x.Date,
                    x.FieldSlotId,
                    x.PlayerId,
                    x.Status
                ))
                .ToListAsync(cancellation);

            return Result.Success<IEnumerable<BookingResponseDto>>(bookings);
        }
    }
}
