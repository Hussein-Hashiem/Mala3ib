namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Follow([FromBody] FollowRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _followService.FollowAsync(userId, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("")]
        public async Task<IActionResult> UnFollow([FromBody] FollowRequestDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _followService.UnFollowAsync(userId, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> Followers([FromRoute] string userId)
        {            
            var result = await _followService.GetFollowersAsync(userId);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> Following([FromRoute] string userId)
        {
            var result = await _followService.GetFollowingAsync(userId);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
