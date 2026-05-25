namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Player)]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("{fieldSlotId}")]
        public async Task<IActionResult> Add([FromRoute] int fieldSlotId, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.AddAsync(userId!, fieldSlotId, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> Cancel([FromRoute] int bookingId, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.CancelAsync(userId!, bookingId, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetById([FromRoute] int bookingId, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.GetByIdAsync(userId!, bookingId, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings(CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.GetMyBookingsAsync(userId!, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
