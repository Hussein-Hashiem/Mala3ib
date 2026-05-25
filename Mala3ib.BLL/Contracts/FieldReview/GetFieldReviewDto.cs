namespace Mala3ib.BLL.Contracts.FieldReview
{
    public record GetFieldReviewDto (
        int Id, 
        string Comment,
        float Rating,
        string UserId,
        string FullName,
        string Email
    );
}
