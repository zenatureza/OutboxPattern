using OutboxPattern.DomainEvents;
using OutboxPattern.Primitives;

namespace OutboxPattern.Entities
{
    public class Activation : AggregateRoot
    {
        public Activation(Guid id) : base(id)
        {
            CreatedAtOnUtc = DateTime.UtcNow;
        }

        public Guid ClientId { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime CreatedAtOnUtc { get; set; }
        public string Serial { get; set; }

        public void Finish(Guid clientId, Guid equipmentId)
        {
            ClientId = clientId;
            EquipmentId = equipmentId;
            RaiseDomainEvent(new ActivationFinishedDomainEvent(clientId, equipmentId));
        }
    }
}
