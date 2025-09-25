namespace SurveyBasket.Services
{
    public interface IPollService
    { 
        Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken); 
        Task<Result> UpdateAsync(int id, PollRequest poll,CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken);
    }
}
