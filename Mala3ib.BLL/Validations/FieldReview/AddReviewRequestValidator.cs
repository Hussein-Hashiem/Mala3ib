using Mala3ib.BLL.Contracts.FieldReview;

namespace Mala3ib.BLL.Validations.FieldReview
{
    public class AddReviewRequestValidator : AbstractValidator<AddReviewRequestDto>
    {
        public AddReviewRequestValidator()
        {
            RuleFor(x => x.Comment)
                .MaximumLength(500);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);
        }
    }
}
