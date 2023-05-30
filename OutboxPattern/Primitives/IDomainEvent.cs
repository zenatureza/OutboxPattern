using MediatR;

namespace OutboxPattern.Primitives
{
    public interface IDomainEvent : INotification
    {
    }
}
