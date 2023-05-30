using MediatR;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Persistence;

namespace OutboxPattern.Application.Handlers.FinishActivation
{
    public class FinishActivationCommandHandler : IRequestHandler<FinishActivationCommand>
    {
        private readonly AppDbContext _appDbContext;

        public FinishActivationCommandHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(FinishActivationCommand request, CancellationToken cancellationToken)
        {
            var activation = 
                await _appDbContext.Activations.FirstOrDefaultAsync(x => x.Id == request.ActivationId);
            activation.Finish(request.ClientId, request.ClientId);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
