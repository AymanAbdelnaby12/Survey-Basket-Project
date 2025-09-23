
using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls=await _pollService.GetAllAsync(cancellationToken);
            var response=polls.Adapt<IEnumerable<PollResponse>>(); 
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var poll =await _pollService.GetByIdAsync(id,cancellationToken);
            if (poll == null)
            {
                return NotFound();
            } 
            var response=poll.Adapt<PollResponse>();
            return Ok(response);

        }

        [HttpPost("")]
        public async Task<IActionResult>Add(CreatePollRequest request,CancellationToken cancellationToken)
        {
            var newPoll =await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll.Adapt<PollResponse>());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreatePollRequest request,CancellationToken cancellationToken) 
        {
            var updatedPoll =await _pollService.UpdateAsync(id, request.Adapt<Poll>(),cancellationToken);
            if (!updatedPoll)  return NotFound(); 

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var deleted = await _pollService.DeleteAsync(id,cancellationToken);
            if (!deleted) return NotFound(); 
            return NoContent();
        }
        [HttpPut("{id}/Toggle")]
        public async Task<IActionResult> Toggle(int id, CancellationToken cancellationToken)
        {
            var updatedPoll = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
            if (!updatedPoll) return NotFound();

            return NoContent();
        }

    }
}
