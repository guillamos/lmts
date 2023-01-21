using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.CommonServices.Abstract;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using MediatR;

namespace LMTS.InputToolSystem.Tools;

public class PlaceBuildingTool: IInputTool
{
    private readonly IInputManager _inputManager;
    
    private readonly PlaceBuildingCommandValidator _placeBuildingCommandValidator;
    private readonly IMediator _mediator;
    private readonly IPathInteractionPointService _pathInteractionPointService;

    private const float MaximumSnapDistance = 2;

    public PlaceBuildingTool(IInputManager inputManager, PlaceBuildingCommandValidator placeBuildingCommandValidator, IMediator mediator, IPathInteractionPointService pathInteractionPointService)
    {
        _inputManager = inputManager;
        _placeBuildingCommandValidator = placeBuildingCommandValidator;
        _mediator = mediator;
        _pathInteractionPointService = pathInteractionPointService;
    }

    public void Activate(string extraData)
    {

    }

    public void Deactivate()
    {

    }

    public void ProcessTick()
    {
        var clickedItems = _inputManager.GetClickedObjectsForTick();

        foreach (var clickedItem in clickedItems)
        {
            if (clickedItem.Node.Name == "FloorPlane")
            {
                var closePoint = _pathInteractionPointService.GetClosestInteractionPoint(clickedItem.PickedPosition, MaximumSnapDistance);

                if (closePoint != null)
                {
                    //todo calculate polygon
                    var placeCommand = new PlaceBuildingCommand(closePoint.Position, new []{ closePoint.Position });

                    var validationResult = _placeBuildingCommandValidator.IsValid(placeCommand);
                    if (validationResult)
                    {
                        _mediator.Send(placeCommand);
                    }
                }
                
                break;
            }
        }
        
    }
}