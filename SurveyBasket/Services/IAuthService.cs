using SurveyBasket.Contract.Auth;

namespace SurveyBasket.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> GetToken(string Email, string Password, CancellationToken cancellationToken);
    }
}
