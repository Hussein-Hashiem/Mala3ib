namespace Mala3ib.BLL.Errors
{
    public static class FollowErrors
    {
        public static Error CannotFollowYourself =
            new("Player.CannotFollowYourself", "You cannot follow yourself.", ErrorType.BadRequest);

        public static readonly Error AlreadyFollowing =
            new("Player.AlreadyFollowing", "You are already following this player.", ErrorType.BadRequest);

        public static readonly Error AlreadyUnfollowed =
            new("Player.AlreadyUnFollowing", "You are already un following this player.", ErrorType.BadRequest);

    }
}
