using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Contract.Auth; 
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtOptions _options;
        public AuthController(IAuthService authService, IOptions<JwtOptions> options)
        {
            _authService = authService;
            _options = options.Value;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login(LoginRequests request,CancellationToken cancellationToken)
        {
            var AuthResponse = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return AuthResponse.IsSuccess ? Ok(AuthResponse.Value)
                : AuthResponse.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return authResult.IsSuccess ? Ok(authResult.Value)
                : authResult.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return isRevoked.IsSuccess ? NoContent()
                : isRevoked.ToProblem(StatusCodes.Status400BadRequest);
        }

    }
}
