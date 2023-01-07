using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.State.WorldState.Abstract;
using LMTS.State.WorldState.Collections;
using MediatR;

namespace LMTS.CommandSystem.Handlers.WorldCommandHandlers;

public class PlaceNavigationPathCommandHandler: IRequestHandler<PlaceNavigationPathCommand>
{
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;
    private readonly PlaceNavigationPathCommandValidator _commandValidator;

    public PlaceNavigationPathCommandHandler(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore, PlaceNavigationPathCommandValidator commandValidator)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
        _commandValidator = commandValidator;
    }

    public Task<Unit> Handle(PlaceNavigationPathCommand request, CancellationToken cancellationToken)
    {
        if (!_commandValidator.IsValid(request))
        {
            //todo should we return something? is any feedback needed?
            return Task.FromResult(Unit.Value);
        }

        var firstJunctionInStore = EnsureJunctionInStore(request.FromJunction);
        var secondJunctionInStore = EnsureJunctionInStore(request.ToJunction);

        var path = firstJunctionInStore.LinkedPaths.Intersect(secondJunctionInStore.LinkedPaths).FirstOrDefault();

        if (path == null)
        {
            path = new WorldNavigationPath(WorldObjectState.Finalized, firstJunctionInStore, secondJunctionInStore, request.PathType);
        }
        
        _pathCollectionStore.Items.Add(path);

        firstJunctionInStore.State = WorldObjectState.Finalized;
        secondJunctionInStore.State = WorldObjectState.Finalized;
        

        return Task.FromResult(Unit.Value);
    }

    public WorldNavigationJunction EnsureJunctionInStore(WorldNavigationJunction junction)
    {
        var junctionInStore = _junctionCollectionStore.Items.FirstOrDefault(j => j == junction || j.Identifier == junction.Identifier);

        if (junctionInStore == null)
        {
            _junctionCollectionStore.Items.Add(junction);
            junctionInStore = junction;
        }

        return junctionInStore;
    }
}