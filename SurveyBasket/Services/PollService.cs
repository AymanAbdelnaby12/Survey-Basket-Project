

using Microsoft.EntityFrameworkCore;
using SurveyBasket.Models;
using SurveyBasket.Persistance;

namespace SurveyBasket.Services
{
    public class PollService : IPollService
    {
        private readonly AppDbContext _dbContext;

        public PollService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken) =>
            await _dbContext.Polls.ToListAsync(cancellationToken);

        public async Task<Poll?> GetByIdAsync(int id,CancellationToken cancellationToken) =>
           await _dbContext.Polls.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);

        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken)
        {
            var existingPoll = await _dbContext.Polls.AnyAsync(x => x.Title == poll.Title, cancellationToken);
            if (existingPoll)
            {
                throw new InvalidOperationException($"A poll with the title '{poll.Title}' already exists.");
            }
            await _dbContext.Polls.AddAsync(poll, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return poll;
        } 
        
        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken)
        {
            var existingPoll = await GetByIdAsync(id, cancellationToken);
            if (existingPoll is null)
            {
                return false;
            }
            existingPoll.Title = poll.Title;
            existingPoll.Description = poll.Description; 
            existingPoll.CreatedAt= poll.CreatedAt;
            existingPoll.EndAt = poll.EndAt;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;

        }

        public async Task<bool> DeleteAsync(int id,CancellationToken cancellationToken)
        {
            var poll = await GetByIdAsync(id,cancellationToken);
            if (poll is null) return false; 

            _dbContext.Remove(poll);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;

        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
        {
            var existingPoll = await GetByIdAsync(id, cancellationToken);
            if (existingPoll is null)
            {
                return false;
            }  
            existingPoll.IsPublished = !existingPoll.IsPublished; 
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
