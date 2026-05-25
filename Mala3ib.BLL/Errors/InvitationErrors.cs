namespace Mala3ib.BLL.Errors
{
    public static class InvitationErrors
    {
        public static Error CannotInviteYourself =
            new("Player.CannotInviteYourself", "You cannot invite yourself.", ErrorType.BadRequest);

        public static readonly Error AlreadyFollowing =
            new("Player.AlreadyFollowing", "You are already inviting this player.", ErrorType.BadRequest);

        public static Error NotFound
            = new Error("Invitation.NotFound", "Invitation not found", ErrorType.NotFound);
        public static Error AlreadyHandled
            = new Error("Invitation.AlreadyHandled", "Invaition already handled", ErrorType.BadRequest);
        public static Error Unauthorized
            = new Error("Invitation.Unauthorized", "User dont have access on this invitation", ErrorType.BadRequest);
        public static Error AlreadySent
            = new Error("Invitation.AlreadySent", "Invitation already sent", ErrorType.BadRequest);
        public static Error AlreadyRequested
            = new Error("Invitation.AlreadyRequested", "Invitation already requested", ErrorType.BadRequest);
        public static Error FieldSlotIsFull
            = new Error("Invitation.FieldSlotIsFull", "FieldSlot Is Full", ErrorType.BadRequest);

    }
}