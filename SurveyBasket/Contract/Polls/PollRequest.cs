namespace SurveyBasket.Contract.Polls
{
    public record PollRequest( 
        string Title,
        string Description, 
        DateOnly CreatedAt,
        DateOnly EndAt 
    ); 
}
