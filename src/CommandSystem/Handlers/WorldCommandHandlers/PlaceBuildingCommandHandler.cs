using System;
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

public class PlaceBuildingCommandHandler: IRequestHandler<PlaceBuildingCommand>
{
    private readonly IWorldStateCollectionStore<WorldBuilding> _buildingCollectionStore;
    private readonly PlaceBuildingCommandValidator _commandValidator;

    public PlaceBuildingCommandHandler(IWorldStateCollectionStore<WorldBuilding> buildingCollectionStore, PlaceBuildingCommandValidator commandValidator)
    {
        _buildingCollectionStore = buildingCollectionStore;
        _commandValidator = commandValidator;
    }

    public Task<Unit> Handle(PlaceBuildingCommand request, CancellationToken cancellationToken)
    {
        if (!_commandValidator.IsValid(request))
        {
            throw new NotImplementedException();
            //todo should we return something? is any feedback needed?
            return Task.FromResult(Unit.Value);
        }
        
        _buildingCollectionStore.Items.Add(new WorldBuilding(WorldObjectState.Finalized, request.OriginPosition, request.PlotPolygon));
        
        return Task.FromResult(Unit.Value);
    }
}