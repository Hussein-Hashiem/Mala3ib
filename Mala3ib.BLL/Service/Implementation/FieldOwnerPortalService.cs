using Mala3ib.BLL.Contracts.Booking;
using Mala3ib.BLL.Contracts.Invitation;
using static System.Net.Mime.MediaTypeNames;

namespace Mala3ib.BLL.Service.Implementation
{
    public class FieldOwnerPortalService : IFieldOwnerPortalService
    {
        private readonly IFieldOwnerRepo _fieldOwnerRepo;
        private readonly IFieldRepo _fieldRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly IInvitationRepo _invitationRepo;

        public FieldOwnerPortalService(IFieldOwnerRepo fieldOwnerRepo, IFieldRepo fieldRepo, IBookingRepo bookingRepo, IInvitationRepo invitationRepo)
        {
            _fieldOwnerRepo = fieldOwnerRepo;
            _fieldRepo = fieldRepo;
            _bookingRepo = bookingRepo;
            _invitationRepo = invitationRepo;
        }

        public async Task<Result<PaginatedList<FieldResponseDto>>> GetMyFieldsAsync(string userId, RequestFilter filter, CancellationToken cancellation = default)
        {
            var owner = await _fieldOwnerRepo.GetOwnerByUserId(userId).FirstOrDefaultAsync(cancellation);
            if (owner is null || owner.IsDeleted)
                return Result.Failure<PaginatedList<FieldResponseDto>>(FieldOwnerErrors.NotFound);

            var query = _fieldRepo.GetByOwnerId(owner.Id)
                .Select(f => new FieldResponseDto(
                    f.Id,
                    f.Name,
                    f.Location,
                    f.PricePerHour,
                    f.Reviews.Select(r => (float?)r.Rating).Average() ?? 0,
                    f.Images
                        .Where(x => !x.IsDeleted)
                        .Select(x => new GetFieldImages(x.Id, x.ImageURL)).ToList()
                ));

            var fields = await PaginatedList<FieldResponseDto>.CreateAsync(query, filter.PageNumber, filter.PageSize, cancellation);
            return Result.Success(fields);
        }

        public async Task<Result<PaginatedList<BookingResponseDto>>> GetMyBookingsAsync(string userId, RequestFilter filter, CancellationToken cancellation = default)
        {
            var owner = await _fieldOwnerRepo.GetOwnerByUserId(userId).FirstOrDefaultAsync(cancellation);
            if (owner is null || owner.IsDeleted)
                return Result.Failure<PaginatedList<BookingResponseDto>>(FieldOwnerErrors.NotFound);

            var query = _bookingRepo.GetByFieldOwnerId(owner.Id)
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

        public async Task<Result<PaginatedList<InvitationResponseDto>>> GetMyInvitationsAsync(string userId, RequestFilter filter, InvitationStatus? status = null, CancellationToken cancellation = default)
        {
            var owner = await _fieldOwnerRepo.GetOwnerByUserId(userId).FirstOrDefaultAsync(cancellation);
            if (owner is null || owner.IsDeleted)
                return Result.Failure<PaginatedList<InvitationResponseDto>>(FieldOwnerErrors.NotFound);

            var query = _invitationRepo.GetByFieldOwnerId(owner.Id, status)
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
    }
}
