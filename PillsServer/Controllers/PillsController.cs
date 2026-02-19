using MediatR;
using Microsoft.AspNetCore.Mvc;
using PillsServer.Common;
using PillsServer.CQRS.Commands;
using PillsServer.CQRS.Queries;

namespace PillsServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PillsController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPills()
        {
            var pills = await Mediator.Send(new GetPillsQuery());
            return Ok(pills);
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPill(int id)
        {
            var pill = await Mediator.Send(new GetPillQuery { Id = id });
            return Ok(pill);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddPill([FromBody] AddPillCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePill([FromBody] UpdatePillCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePill([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeletePillCommand { Id=id});
            return Ok(response);
        }
        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> TakePill([FromRoute] int id)
        {
            var response = await Mediator.Send(new TakePillCommand { Id=id});
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> TakeAllPills()
        {
            var response = await Mediator.Send(new TakeAllPillsCommand());
            return Ok(response);
        }

    }
}
