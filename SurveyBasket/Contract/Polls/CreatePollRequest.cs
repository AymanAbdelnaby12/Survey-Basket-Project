namespace SurveyBasket.Contract.Polls
{
    public record CreatePollRequest( 
        string Title,
        string Description, 
        DateOnly CreatedAt,
        DateOnly EndAt 
    ); 
}
