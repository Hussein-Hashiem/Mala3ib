using Mala3ib.BLL.Contracts.Admin;
using Mala3ib.BLL.Contracts.Booking;
using Mala3ib.BLL.Contracts.Invitation;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IAdminService
    {
        Task<Result<PaginatedList<BookingResponseDto>>> GetAllBookingsAsync(RequestFilter filter, CancellationToken cancellation = default);
        Task<Result<PaginatedList<InvitationResponseDto>>> GetAllInvitationsAsync(RequestFilter filter, InvitationStatus? status = null, CancellationToken cancellation = default);
        Task<Result<PaginatedList<AdminFieldOwnerDto>>> GetFieldOwnersAsync(RequestFilter filter, Status? status = null, CancellationToken cancellation = default);
        Task<Result> UpdateFieldOwnerStatusAsync(string ownerUserId, Status status, CancellationToken cancellation = default);
    }
}
