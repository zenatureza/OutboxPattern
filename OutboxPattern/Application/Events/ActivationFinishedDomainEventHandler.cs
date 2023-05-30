using MediatR;
using OutboxPattern.DomainEvents;

namespace OutboxPattern.Application.Events
{
    public class ActivationFinishedDomainEventHandler : INotificationHandler<ActivationFinishedDomainEvent>
    {
        public Task Handle(ActivationFinishedDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("ActivationFinishedDomainEventHandler");
            Console.WriteLine(notification);

            return Task.CompletedTask;
        }
    }
}
