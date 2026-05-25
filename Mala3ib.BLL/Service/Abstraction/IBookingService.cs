using Mala3ib.BLL.Contracts.Booking;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IBookingService
    {
        Task<Result> AddAsync(string currentUserId, int fieldSlotId, CancellationToken cancellation = default);
        Task<Result> CancelAsync(string currentUserId, int bookingId, CancellationToken cancellation = default);
        Task<Result<BookingResponseDto>> GetByIdAsync(string currentUserId, int bookingId, CancellationToken cancellation = default);
        Task<Result<IEnumerable<BookingResponseDto>>> GetMyBookingsAsync(string currentUserId, CancellationToken cancellation = default);
    }
}
