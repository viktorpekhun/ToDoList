using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Features.Todos.Commands;
using ToDoList.Application.Features.Todos.Queries;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] GetToDoItemsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetToDoItemByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Error.Code.Contains("NotFound"))
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddToDoItemCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateToDoItemCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "ID у шляху не збігається з ID у тілі запиту." });
            }

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Error.Code.Contains("NotFound"))
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteToDoItemCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Error.Code.Contains("NotFound"))
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}
