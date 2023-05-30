using MediatR;

namespace OutboxPattern.Application.Handlers.FinishActivation
{
    public class FinishActivationCommand : IRequest
    {
        public FinishActivationCommand(Guid activationId, Guid clientId, Guid equipmentId)
        {
            ActivationId = activationId;
            ClientId = clientId;
            EquipmentId = equipmentId;
        }

        public Guid ActivationId { get; }
        public Guid ClientId { get; }
        public Guid EquipmentId { get; }
    }
}
