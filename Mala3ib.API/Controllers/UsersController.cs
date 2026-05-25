namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public UsersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPlayers([FromQuery] RequestFilter filter, CancellationToken cancellationToken)
        {
            var players = await _playerService.GetAllAsync(filter, cancellationToken);

            return Ok(players);
        }

    }
}
