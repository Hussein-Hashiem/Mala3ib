namespace Mala3ib.BLL.Validations.FieldReview
{
    public class UpdateFieldReviewValidator : AbstractValidator<UpdateFieldReviewDto>
    {
        public UpdateFieldReviewValidator()
        {
            RuleFor(x => x.Comment)
                .MaximumLength(500);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);
        }
    }
}
