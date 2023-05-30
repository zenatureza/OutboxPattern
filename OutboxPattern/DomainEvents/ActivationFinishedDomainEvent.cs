using OutboxPattern.Primitives;

namespace OutboxPattern.DomainEvents
{
    public sealed record ActivationFinishedDomainEvent(Guid ClientId, Guid EquipmentId) : IDomainEvent
    {
    }
}
