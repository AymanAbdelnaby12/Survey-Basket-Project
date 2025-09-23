using FluentValidation;

namespace SurveyBasket.Contract.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequests>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6); 
    } 
}