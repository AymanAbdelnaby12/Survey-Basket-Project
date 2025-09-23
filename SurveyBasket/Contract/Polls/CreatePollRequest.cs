namespace SurveyBasket.Contract.Polls
{
    public record CreatePollRequest( 
        string Title,
        string Description,
        bool IsPublished,
        DateOnly CreatedAt,
        DateOnly EndAt 
    ); 
}
