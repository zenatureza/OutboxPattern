using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OutboxPattern.Persistence;
using OutboxPattern.Persistence.Outbox;
using OutboxPattern.Primitives;
using Quartz;

namespace OutboxPattern.Jobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessagesJob(AppDbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _dbContext
                .Set<OutboxMessage>()
                .Where(x => x.ProcessedOnUtc == null)
                .Take(20)
                .ToListAsync(context.CancellationToken);

            foreach (var message in messages) 
            {
                try
                {
                    var domainEvent = 
                        JsonConvert.DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                    if (domainEvent is null) continue;

                    await _publisher.Publish(domainEvent, context.CancellationToken);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
