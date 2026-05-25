namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FieldOwner}")]
    public class FieldOwnerController : ControllerBase
    {
        private readonly IFieldOwnerService _fieldOwnerService;
        public FieldOwnerController(IFieldOwnerService fieldOwnerService)
        {
            _fieldOwnerService = fieldOwnerService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _fieldOwnerService.GetDashboardAsync(ownerId!, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

    }
}
