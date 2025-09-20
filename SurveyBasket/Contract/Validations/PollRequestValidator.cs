using FluentValidation;
using SurveyBasket.Contract.Request;

namespace SurveyBasket.Contracts.Validations;

public class PollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Please enter A Title")
            .Length(3, 100);

        
    }
}