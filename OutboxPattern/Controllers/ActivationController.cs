using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Application.Handlers.CreateActivation;
using OutboxPattern.Application.Handlers.FinishActivation;
using OutboxPattern.Entities;
using OutboxPattern.Persistence;
using OutboxPattern.Persistence.Outbox;

namespace OutboxPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _dbContext;

        public ActivationController(IMediator mediator, AppDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var activations = await _dbContext.Activations.ToListAsync();
            return Ok(activations);
        }

        [HttpGet("events")]
        public async Task<IActionResult> Events()
        {
            var events = await _dbContext.Set<OutboxMessage>().ToListAsync();
            return Ok(events);
        }

        [HttpPost("create")]
        public async Task Create([FromBody] CreateActivationCommand command)
        {
            await _dbContext.AddAsync(new Activation(Guid.NewGuid())
            {
                Serial = command.Serial,
            });

            await _dbContext.SaveChangesAsync();
        }

        [HttpPost("finish")]
        public async Task Finish([FromBody] FinishActivationCommand command)
        {
            await _mediator.Send(command);
        }
    }
}
