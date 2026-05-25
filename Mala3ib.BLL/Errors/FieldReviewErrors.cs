namespace Mala3ib.BLL.Errors
{
    public class FieldReviewErrors
    {
        public static Error HasReview
            = new Error("FieldReview.HasReview", "already Reviewed", ErrorType.BadRequest);

        public static Error NotFound
            = new Error("FieldReview.NotFound", "Review not found", ErrorType.BadRequest);

        public static Error Unauthorized
            = new Error("FieldReview.Unauthorized", "You are not authorized to modify this review", ErrorType.Unauthorized);
    }
}
