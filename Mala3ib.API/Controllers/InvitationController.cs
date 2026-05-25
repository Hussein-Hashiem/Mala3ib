using Mala3ib.BLL.Contracts.Invitation;
using Mala3ib.DAL.Enums;

namespace Mala3ib.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }
        [HttpPost("Invite/")]
        public async Task<IActionResult> Invite([FromBody] SendInviationDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _invitationService.SendAsync(userId, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPost("Request/{fieldSlotId}")]
        public async Task<IActionResult> request([FromRoute] int fieldSlotId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _invitationService.RequestAsync(userId, fieldSlotId);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPost("Accept/{invitationId}")]
        public async Task<IActionResult> Accept([FromRoute] int invitationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _invitationService.AcceptAsync(userId, invitationId);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPost("Reject/{invitationId}")]
        public async Task<IActionResult> Reject([FromRoute] int invitationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _invitationService.RejectAsync(userId, invitationId);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _invitationService.DeleteAsync(id, userId!, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpGet("recieved")]
        public async Task<IActionResult> GetRecievedInvitations([FromQuery] InvitationStatus status, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _invitationService.GetRecievedInvitations(userId!, status, cancellation);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("sent")]
        public async Task<IActionResult> GetSentInvitations([FromQuery] InvitationStatus status, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _invitationService.GetSentInvitations(userId!, status, cancellation);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}