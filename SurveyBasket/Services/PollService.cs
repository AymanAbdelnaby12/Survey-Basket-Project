
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Errors; 

namespace SurveyBasket.Services
{
    public class PollService : IPollService
    {
        private readonly AppDbContext _dbContext;

        public PollService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Polls
                .AsNoTracking()
                .ProjectToType<PollResponse>()
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken)
        {

            return await _dbContext.Polls
                .Where(x=>x.IsPublished && x.CreatedAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .ProjectToType<PollResponse>()
                .ToListAsync(cancellationToken);
        }
        public async Task<Result<PollResponse>> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            var poll = await _dbContext.Polls.FindAsync( id,cancellationToken);
            if (poll is null)
            {
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);
            }
            var response = poll.Adapt<PollResponse>();
            return Result.Success(response);

        }

        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken)
        {
            var existingPoll = await _dbContext.Polls.AnyAsync(x => x.Title == request.Title, cancellationToken);
            if (existingPoll)
               return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

            var newPoll = request.Adapt<Poll>();
            _dbContext.Polls.Add(newPoll);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(newPoll.Adapt<PollResponse>());

        } 
        
        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken)
        {
            var isExistingTitle = await _dbContext.Polls.AnyAsync(x => x.Title == request.Title && x.Id != id, cancellationToken: cancellationToken);

            if (isExistingTitle)
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

            var currentPoll = await _dbContext.Polls.FindAsync(id, cancellationToken);

            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);

            currentPoll.Title = request.Title;
            currentPoll.Description = request.Description;
            currentPoll.CreatedAt = request.CreatedAt;
            currentPoll.EndAt = request.EndAt;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> DeleteAsync(int id,CancellationToken cancellationToken)
        {
            var poll = await _dbContext.Polls.FindAsync(id, cancellationToken);
            if (poll is null)
            {
                return Result.Failure<bool>(PollErrors.PollNotFound);
            }
             _dbContext.Polls.Remove(poll);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
        {
            var existingPoll = await _dbContext.Polls.FindAsync(id, cancellationToken);
            if (existingPoll is null)
            {
                return Result.Failure(PollErrors.PollNotFound);
            }   
            existingPoll.IsPublished = !existingPoll.IsPublished;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
