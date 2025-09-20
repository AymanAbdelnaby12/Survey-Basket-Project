namespace SurveyBasket.Contract.Response
{
    public record PollResponse(
        int Id,
        string Title,
        string Description
    );
}
