using Mala3ib.BLL.Contracts.Admin;
using Mala3ib.BLL.Contracts.Booking;
using Mala3ib.BLL.Contracts.Invitation;

namespace Mala3ib.BLL.Service.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IInvitationRepo _invitationRepo;
        private readonly IFieldOwnerRepo _fieldOwnerRepo;

        public AdminService(IBookingRepo bookingRepo, IInvitationRepo invitationRepo, IFieldOwnerRepo fieldOwnerRepo)
        {
            _bookingRepo = bookingRepo;
            _invitationRepo = invitationRepo;
            _fieldOwnerRepo = fieldOwnerRepo;
        }

        public async Task<Result<PaginatedList<BookingResponseDto>>> GetAllBookingsAsync(RequestFilter filter, CancellationToken cancellation = default)
        {
            var query = _bookingRepo.GetAll()
                .Select(x => new BookingResponseDto(
                    x.Id,
                    x.Date,
                    x.FieldSlotId,
                    x.PlayerId,
                    x.Status
                ));

            var bookings = await PaginatedList<BookingResponseDto>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellation);

            return Result.Success(bookings);
        }

        public async Task<Result<PaginatedList<InvitationResponseDto>>> GetAllInvitationsAsync(RequestFilter filter, InvitationStatus? status = null, CancellationToken cancellation = default)
        {
            var query = _invitationRepo.GetAll(status)
                .Select(i => new InvitationResponseDto(
                    i.SenderId,
                    i.RecieverId,
                    i.FieldSlotId,
                    i.Status,
                    i.Type
                ));

            var invitations = await PaginatedList<InvitationResponseDto>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellation);

            return Result.Success(invitations);
        }

        public async Task<Result<PaginatedList<AdminFieldOwnerDto>>> GetFieldOwnersAsync(RequestFilter filter, Status? status = null, CancellationToken cancellation = default)
        {
            var query = _fieldOwnerRepo.GetAll(status)
                .Select(x => new AdminFieldOwnerDto(
                    x.Id,
                    x.UserId,
                    x.User.Email!,
                    x.User.FirstName,
                    x.User.LastName,
                    x.User.PhoneNumber,
                    x.DateOfBirth,
                    x.Status
                ));

            var owners = await PaginatedList<AdminFieldOwnerDto>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellation);

            return Result.Success(owners);
        }

        public async Task<Result> UpdateFieldOwnerStatusAsync(string ownerUserId, Status status, CancellationToken cancellation = default)
        {
            var exists = await _fieldOwnerRepo.FieleOwnerIsExist(ownerUserId, cancellation);
            if (!exists)
                return Result.Failure(FieldOwnerErrors.NotFound);

            await _fieldOwnerRepo.UpdateStatusAsync(ownerUserId, status, cancellation);
            return Result.Success();
        }
    }
}
