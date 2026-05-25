namespace Mala3ib.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellation)
        {
            var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellation);

            return result!.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            var refreshResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return refreshResult!.IsSuccess ? Ok(refreshResult.Value) : refreshResult.ToProblem();  
        }

        [HttpPut("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefresh([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("player-register")]
        public async Task<IActionResult> PlayerRegistration([FromBody] RegisterPlayerDto request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterPlayerAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("field-owner-register")]
        public async Task<IActionResult> FieldOwnerRegistration([FromBody] RegisterFieldOwnerDto request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterFieldOwnerAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDto request)
        {
            var result = await _authService.ConfirmEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequestDto request)
        {
            var result = await _authService.ResendConfirmationEmailAsync(request);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto request)
        {
            var result = await _authService.SendResetPasswordCodeAsync(request.Email);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("verify-reset-password-otp")]
        public async Task<IActionResult> VerifyResetPasswordOtp([FromBody] VerifyResetPasswordOtpRequestDto request)
        {
            var result = await _authService.VerifyResetPasswordOtpAsync(request.Email, request.Otp);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
