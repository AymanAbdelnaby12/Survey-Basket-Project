namespace SurveyBasket.Contract.Auth;
public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);