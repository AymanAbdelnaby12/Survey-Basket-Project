namespace SurveyBasket.Services
{
    public interface IPollService
    { 
        Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken);
        Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Poll> AddAsync(Poll poll,CancellationToken cancellationToken); 
        Task<bool> UpdateAsync(int id, Poll poll,CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken);
    }
}
