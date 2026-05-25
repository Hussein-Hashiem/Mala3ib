namespace Mala3ib.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldSlotController : ControllerBase
    {
        private readonly IFieldSlotService _fieldSlotService;
        public FieldSlotController(IFieldSlotService fieldSlotService)
        {
            _fieldSlotService = fieldSlotService;
        }

        [Authorize(Roles = DefaultRoles.FieldOwner)]
        [HttpPost("{fieldId}")]
        public async Task<IActionResult> Add([FromRoute] int fieldId, [FromBody] AddFieldSlotRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _fieldSlotService.AddAsync(request, userId!, fieldId, cancellation);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.FieldOwner)]
        [HttpPut("{fieldId}/slot/{id}")]
        public async Task<IActionResult> Update([FromRoute] int fieldId, [FromRoute] int id, UpdateFieldSlotRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _fieldSlotService.UpdateAsync(id, request, userId!, fieldId, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.FieldOwner)]
        [HttpDelete("{fieldId}/slot/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int fieldId, [FromRoute] int id, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _fieldSlotService.DeleteAsync(id, fieldId, userId!, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellation)
        {
            var result = await _fieldSlotService.GetByIdAsync(id, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpGet("field/{fieldId}")]
        public async Task<IActionResult> GetByFieldId([FromRoute] int fieldId, CancellationToken cancellation)
        {
            var result = await _fieldSlotService.GetByFieldIdAsync(fieldId, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpGet("field/{fieldId}/avialable-slots")]
        public async Task<IActionResult> GetFieldSlots([FromRoute] int fieldId, [FromQuery] DateTime? date, CancellationToken cancellation)
        {
            var result = await _fieldSlotService.GetAvailableSlots(fieldId, date, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}