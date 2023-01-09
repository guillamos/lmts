using System.Threading;
using System.Threading.Tasks;
using LMTS.CommandSystem.Commands.WorldCommands;
using MediatR;

namespace LMTS.CommandSystem.Handlers.WorldCommandHandlers;

public class PlaceBuildingCommandHandler: IRequestHandler<PlaceBuildingCommand>
{
    public Task<Unit> Handle(PlaceBuildingCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Unit.Value);
    }
}