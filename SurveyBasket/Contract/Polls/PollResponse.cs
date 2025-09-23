namespace SurveyBasket.Contract.Polls
{
    public record PollResponse(
        int Id,
        string Title,
        string Description,
        bool IsPublished,
        DateOnly CreatedAt,
        DateOnly EndAt 
    );
}
