using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contract.Auth;

namespace SurveyBasket.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponse> GetToken(string Email, string Password, CancellationToken cancellationToken)
        {
            // Check if Email Exist
            var user=await _userManager.FindByEmailAsync(Email);
            if(user==null) return null;

            // Check if Password is correct
            var isPasswordCorrect=await _userManager.CheckPasswordAsync(user,Password);
            if(!isPasswordCorrect) return null;

            // Generate Token
            var (token,expiration)=_jwtProvider.GenerateToken(user);

            // return AuthResponse 
            return new AuthResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                token,
                expiration
                );
        }
    }
}
