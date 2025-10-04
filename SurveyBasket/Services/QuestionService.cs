﻿
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Answers;
using SurveyBasket.Errors;
using System.Linq;

namespace SurveyBasket.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _dbContext;

        public QuestionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _dbContext.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);

            if (!pollIsExists)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

            var questions = await _dbContext.Questions
                .Where(x => x.PollId == pollId)
                .Include(x => x.Answers)
                //.Select(q => new QuestionResponse(
                //    q.Id,
                //    q.Content,
                //    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))
                //))
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _dbContext.Questions
                .Where(x => x.PollId == pollId && x.Id == id)
                .Include(x => x.Answers)
                .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);

            return Result.Success(question);
        }

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _dbContext.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

            var questionIsExists = await _dbContext.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken: cancellationToken);

            if (questionIsExists)
                return Result.Failure<QuestionResponse>(QuestionError.DuplicatedQuestionContent);

            var question = request.Adapt<Question>();
            question.PollId = pollId;

            await _dbContext.AddAsync(question, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var questionIsExists = await _dbContext.Questions
                .AnyAsync(x => x.PollId == pollId
                    && x.Id != id
                    && x.Content == request.Content,
                    cancellationToken
                );

            if (questionIsExists)
                return Result.Failure(QuestionError.DuplicatedQuestionContent);

            var question = await _dbContext.Questions
                .Include(x => x.Answers)
                .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

            if (question is null)
                return Result.Failure(QuestionError.QuestionNotFound);

            question.Content = request.Content;

            //current answers
            var currentAnswers = question.Answers.Select(x => x.Content).ToList();

            //add new answer
            var newAnswers = request.Answers.Except(currentAnswers).ToList();

            newAnswers.ForEach(answer =>
            {
                question.Answers.Add(new Answer { Content = answer });
            });

            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _dbContext.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

            if (question is null)
                return Result.Failure(QuestionError.QuestionNotFound);

            question.IsActive = !question.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}