using Mala3ib.BLL.Contracts.Admin;
using Mala3ib.DAL.Enums;

namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings([FromQuery] RequestFilter filter, CancellationToken cancellation)
        {
            var result = await _adminService.GetAllBookingsAsync(filter, cancellation);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetInvitations([FromQuery] RequestFilter filter, [FromQuery] InvitationStatus? status, CancellationToken cancellation)
        {
            var result = await _adminService.GetAllInvitationsAsync(filter, status, cancellation);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("field-owners")]
        public async Task<IActionResult> GetFieldOwners([FromQuery] RequestFilter filter, [FromQuery] Status? status, CancellationToken cancellation)
        {
            var result = await _adminService.GetFieldOwnersAsync(filter, status, cancellation);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("field-owners/{userId}/status")]
        public async Task<IActionResult> UpdateFieldOwnerStatus([FromRoute] string userId, [FromBody] UpdateFieldOwnerStatusRequest request, CancellationToken cancellation)
        {
            var result = await _adminService.UpdateFieldOwnerStatusAsync(userId, request.Status, cancellation);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
