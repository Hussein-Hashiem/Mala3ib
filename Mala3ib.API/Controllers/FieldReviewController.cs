namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FieldReviewController : ControllerBase
    {
        private readonly IFieldReviewService _fieldReviewService;

        public FieldReviewController(IFieldReviewService fieldReviewService)
        {
            _fieldReviewService = fieldReviewService;
        }

        [HttpPost("{fieldId}")]
        [Authorize(Roles = DefaultRoles.Player)]
        public async Task<IActionResult> Add([FromRoute] int fieldId, [FromBody] AddReviewRequestDto request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fieldReviewService.AddReviewAsync(userId!, fieldId, request, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPut("{reviewId}")]
        [Authorize(Roles = DefaultRoles.Player)]
        public async Task<IActionResult> Update([FromRoute] int reviewId, [FromBody] UpdateFieldReviewDto request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fieldReviewService.UpdateReviewAsync(userId!, reviewId, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{reviewId}")]
        [Authorize(Roles = $"{DefaultRoles.Player},{DefaultRoles.Admin}")]
        public async Task<IActionResult> Delete([FromRoute] int reviewId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(DefaultRoles.Admin);
            var result = await _fieldReviewService.DeleteAsync(userId!, reviewId, isAdmin, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("{reviewId}")]
        [Authorize]
        public async Task<IActionResult> GetReview([FromRoute] int reviewId, CancellationToken cancellationToken)
        {
            var result = await _fieldReviewService.GetReviewAsync(reviewId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{fieldId}/reviews")]
        [Authorize]
        public async Task<IActionResult> GetReviewsForField([FromRoute] int fieldId, [FromQuery] RequestFilter filter, CancellationToken cancellationToken)
        {
            var result = await _fieldReviewService.GetAllReviewsForFieldAsync(fieldId, filter, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
