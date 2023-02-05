using System.Threading;
using System.Threading.Tasks;
using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.State.WorldState.Abstract;
using MediatR;

namespace LMTS.CommandSystem.Handlers.WorldCommandHandlers;

public class AddTripCommandHandler: IRequestHandler<AddTripCommand>
{
    private readonly IWorldStateCollectionStore<WorldTrip> _tripCollectionStore;

    public AddTripCommandHandler(IWorldStateCollectionStore<WorldTrip> tripCollectionStore)
    {
        _tripCollectionStore = tripCollectionStore;
    }

    public Task<Unit> Handle(AddTripCommand request, CancellationToken cancellationToken)
    {
        var mappedWorldTrip = new WorldTrip(WorldObjectState.Finalized, request.From, request.To, request.TripItinerary);
        
        _tripCollectionStore.Items.Add(mappedWorldTrip);
        
        return Task.FromResult(Unit.Value);
    }
}