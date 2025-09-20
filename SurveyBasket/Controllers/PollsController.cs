
using Mapster;
using SurveyBasket.Contract.Request;
using SurveyBasket.Contract.Response;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var polls=_pollService.GetAll();
            var response=polls.Adapt<IEnumerable<Poll>>(); 
            return Ok(response);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var poll = _pollService.Get(id);
            if (poll == null)
            {
                return NotFound();
            } 
            var response=poll.Adapt<PollResponse>();
            return Ok(response);

        }

        [HttpPost("")]
        public IActionResult Add(CreatePollRequest request)
        {
            var newPoll = _pollService.Add(request.Adapt<Poll>());

            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
        }
        [HttpPut]
        public IActionResult Update(int id , CreatePollRequest request) 
        {
            var isUpdated=_pollService.Update(id, request.Adapt<Poll>());
            if(!isUpdated) return NotFound();
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var poll =_pollService.Delete(id);
            if (!poll)
            {
                return NotFound();
            }
            return NoContent();
        }
       
    }
}
