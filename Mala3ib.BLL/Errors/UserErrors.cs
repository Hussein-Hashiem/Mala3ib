namespace Mala3ib.BLL.Errors
{
    public static class UserErrors
    {
        public static Error InvalidCredentials
            = new Error("User.InvalidCredentials", "Invalid email or password", ErrorType.Unauthorized);

        public static readonly Error InvalidTokens
            = new Error("User.InvalidCredentials", "Invalid token or resfresh token", ErrorType.Unauthorized);

        public static readonly Error DuplicatedEmail
            = new Error("User.DuplicatedEmail", "Another user with the same emial is already exists", ErrorType.Conflict);

        public static readonly Error EmailNotConfirmed
            = new Error("User.EmailNotConfirmed", "Email is not confirmed", ErrorType.Unauthorized);

        public static readonly Error InvalidCode
            = new Error("User.InvalidCode", "Invalid code", ErrorType.Unauthorized);

        public static readonly Error DuplicatedConfirmation
            = new Error("User.DuplicatedConfirmation", "Email is already confirmed", ErrorType.BadRequest);

        public static readonly Error NotFouond
            = new Error("User.NotFound", "User is not found", ErrorType.BadRequest);

        public static readonly Error FieldOwnerPending
            = new Error("User.FieldOwnerPending", "Your account is still pending approval", ErrorType.BadRequest);

        public static readonly Error FieldOwnerRejected
            = new Error("User.FieldOwnerRejected", "Your account has been rejected", ErrorType.BadRequest);
    }
}
