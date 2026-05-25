using Mala3ib.BLL.Contracts.Booking;
using Mala3ib.BLL.Contracts.Field;
using Mala3ib.BLL.Contracts.Invitation;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFieldOwnerPortalService
    {
        Task<Result<PaginatedList<FieldResponseDto>>> GetMyFieldsAsync(string userId, RequestFilter filter, CancellationToken cancellation = default);
        Task<Result<PaginatedList<BookingResponseDto>>> GetMyBookingsAsync(string userId, RequestFilter filter, CancellationToken cancellation = default);
        Task<Result<PaginatedList<InvitationResponseDto>>> GetMyInvitationsAsync(string userId, RequestFilter filter, InvitationStatus? status = null, CancellationToken cancellation = default);
    }
}
