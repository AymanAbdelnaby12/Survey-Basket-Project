using FluentValidation;

namespace SurveyBasket.Contract.Polls;

public class LoginRequestValidator : AbstractValidator<PollRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty() 
            .WithMessage("Please enter A Title")
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x)
            .Must(HasValidDates)
            .WithName(nameof(PollRequest.EndAt))
            .WithMessage("{PropertyName} must be greater than or equals start date");
    }
    private bool HasValidDates(PollRequest pollRequest)
    {
        return pollRequest.EndAt >= pollRequest.CreatedAt;
    }
}