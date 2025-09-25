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
            var pollResult =await _pollService.GetByIdAsync(id,cancellationToken);
           
            if (pollResult.IsSuccess) return Ok(pollResult.Value);
            return pollResult.ToProblem(StatusCodes.Status404NotFound);
        }
         
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest request,
        CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(request, cancellationToken);
            if (newPoll.IsSuccess) return CreatedAtAction(nameof(Get), new { id = newPoll.Value.Id }, newPoll.Value);
            return newPoll.ToProblem(StatusCodes.Status400BadRequest);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest request,CancellationToken cancellationToken) 
        {
            var updatedPoll =await _pollService.UpdateAsync(id, request,cancellationToken);
            if (updatedPoll.IsSuccess)
                return NoContent();

            return updatedPoll.ToProblem(StatusCodes.Status404NotFound);
           
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var deleted = await _pollService.DeleteAsync(id,cancellationToken);
            if (deleted.IsSuccess) return NoContent();
            return deleted.ToProblem(StatusCodes.Status404NotFound);
        }
        [HttpPut("{id}/Toggle")]
        public async Task<IActionResult> Toggle(int id, CancellationToken cancellationToken)
        {
            var updatedPoll = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
            if (updatedPoll.IsSuccess)
                return NoContent();
            return updatedPoll.ToProblem(StatusCodes.Status404NotFound);

        }

    }
}
