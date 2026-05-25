namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IFieldOwnerService _fieldOwnerService;
        private readonly IFileService _fileService;
        public AccountController(IPlayerService playerService, IFieldOwnerService fieldOwnerService, IFileService fileService)
        {
            _playerService = playerService;
            _fieldOwnerService = fieldOwnerService;
            _fileService = fileService;
        }

        [Authorize]
        [HttpGet("player/{userId}")]
        public async Task<IActionResult> GetPlayer([FromRoute] string userId, CancellationToken cancellation)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _playerService.GetAsync(currentUserId!, userId, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpGet("field-owner/{userId}")]
        public async Task<IActionResult> GetOwner([FromRoute] string userId, CancellationToken cancellation)
        {
            var result = await _fieldOwnerService.GetAsync(userId, cancellation);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.Player)]
        [HttpPut("player/change-password")]
        public async Task<IActionResult> ChangePlayerPassword([FromBody] ChangePasswordRequestDto request)
        {
            var uesrId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _playerService.ChangePasswordAsync(uesrId!, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.FieldOwner)]
        [HttpPut("field-owner/change-password")]
        public async Task<IActionResult> ChangeOwnerPassword([FromBody] ChangePasswordRequestDto request)
        {
            var uesrId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fieldOwnerService.ChangePasswordAsync(uesrId!, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.Player)]
        [HttpDelete("player")]
        public async Task<IActionResult> DeletePlayer(CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _playerService.DeleteAsync(userId!);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        // return to this endpoint 
        [Authorize(Roles = $"{DefaultRoles.FieldOwner},{DefaultRoles.Admin}")]
        [HttpDelete("field-owner/{id}")]
        public async Task<IActionResult> DeleteOwner([FromRoute] string id, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fieldOwnerService.DeleteAsync(userId!);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.Player)]
        [HttpPut("player")]
        public async Task<IActionResult> UpdatePlayer(UpdatePlayerRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _playerService.UpdateAsync(userId!, request, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.FieldOwner)]
        [HttpPut("field-owner")]
        public async Task<IActionResult> UpdateOwner(UpdateFieldOwnerRequestDto request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _fieldOwnerService.UpdateAsync(userId!, request, cancellation);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("upload-profile-image")]
        [Authorize]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request, CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _fileService.UploadImageAsync(userId!, request.Image, cancellation);

            return Created();
        }

        [HttpDelete("delete-profile-image")]
        [Authorize]
        public async Task<IActionResult> DeleteProfileImage(CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _fileService.DeleteProfileImageAsync(userId!, cancellationToken);

            return NoContent();
        }
    }
}
