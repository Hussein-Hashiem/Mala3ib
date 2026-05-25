namespace Mala3ib.DAL.Repo.Implementation
{
    public class InvitationRepo : IInvitationRepo
    {
        private readonly ApplicationDbContext _context;
        public InvitationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> InviteAsync(int sendPlayerId, int targetPlayerId, int fieldSlotId, InvitationType type, CancellationToken cancellation)
        {
            var invitation = await _context.Invitations
                .FirstOrDefaultAsync(x => x.SenderId == sendPlayerId && x.RecieverId == targetPlayerId && x.FieldSlotId == fieldSlotId, cancellation);

            if (invitation is not null)
            {
                if (!invitation.IsDeleted)
                    return false;

                invitation.IsDeleted = false;
                invitation.Status = InvitationStatus.Pending;
                invitation.CreatedAt = DateTime.UtcNow;
                invitation.Type = type;

                await _context.SaveChangesAsync(cancellation);
                return true;
            }

            var newInviation = new Invitation
            {
                SenderId = sendPlayerId,
                RecieverId = targetPlayerId,
                FieldSlotId = fieldSlotId,
                CreatedAt = DateTime.UtcNow,
                Type = type
            };

            await _context.Invitations.AddAsync(newInviation);
            await _context.SaveChangesAsync(cancellation);

            return true;
        }

        public IQueryable<Invitation> GetAll(InvitationStatus? status = null)
        {
            var query = _context.Invitations
                .Where(x => !x.IsDeleted)
                .AsNoTracking();

            if (status is not null)
                query = query.Where(x => x.Status == status);

            return query;
        }

        public IQueryable<Invitation> GetByFieldOwnerId(int fieldOwnerId, InvitationStatus? status = null)
        {
            var query = _context.Invitations
                .Where(x => !x.IsDeleted && x.FieldSlot.Field.FieldOwnerId == fieldOwnerId)
                .AsNoTracking();

            if (status is not null)
                query = query.Where(x => x.Status == status);

            return query;
        }

        public IQueryable<Invitation> GetById(int id)
        {
            var invitation = _context.Invitations
                .Where(x => x.Id == id && !x.IsDeleted)
                .AsNoTracking();

            return invitation;
        }

        public IQueryable<Invitation> GetReceivedInvitations(int id, InvitationStatus status)
        {
            var invitation = _context.Invitations
                .Where(x => x.RecieverId == id && !x.IsDeleted && x.Status == status)
                .AsNoTracking();

            return invitation;
        }

        public IQueryable<Invitation> GetSentInvitations(int id, InvitationStatus status)
        {
            var invitation = _context.Invitations
                .Where(x => x.SenderId == id && !x.IsDeleted && x.Status == status)
                .AsNoTracking();

            return invitation;
        }

        public async Task UpdateAsync(Invitation invitation, CancellationToken cancellation = default)
        {
            await _context.Invitations
                .Where(f => f.Id == invitation.Id)
                .ExecuteUpdateAsync(setter =>
                    setter.SetProperty(x => x.Status, invitation.Status), cancellation
                );
        }

        public async Task DeletePendingByFieldSlotIdAsync(int fieldSlotId, CancellationToken cancellation = default)
        {
            await _context.Invitations
                .Where(x => x.FieldSlotId == fieldSlotId && !x.IsDeleted && x.Status == InvitationStatus.Pending)
                .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsDeleted, true), cancellation);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            var affectedRows = await _context.Invitations
                .Where(x => x.Id == id && !x.IsDeleted)
                .ExecuteUpdateAsync(setter =>
                setter.SetProperty(x => x.IsDeleted, true)
                );
        }

    }
}