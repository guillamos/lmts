using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using MediatR;

namespace LMTS.InputToolSystem.Tools;

public class PlaceBuildingTool: IInputTool
{
    private readonly IInputManager _inputManager;
    
    private readonly PlaceBuildingCommandValidator _placeBuildingCommandValidator;
    private readonly IMediator _mediator;

    public PlaceBuildingTool(IInputManager inputManager, PlaceBuildingCommandValidator placeBuildingCommandValidator, IMediator mediator)
    {
        _inputManager = inputManager;
        _placeBuildingCommandValidator = placeBuildingCommandValidator;
        _mediator = mediator;
    }

    public void Activate(string extraData)
    {

    }

    public void Deactivate()
    {

    }

    public void ProcessTick()
    {
        var clickedItems = _inputManager.GetPickedObjectsForTick();

        if (clickedItems == null)
        {
            return;
        }

        foreach (var clickedItem in clickedItems)
        {
            if (clickedItem.Node.Name == "FloorPlane")
            {
                //create a new junction on this position
                //todo: check if this is a valid position for a junction
                var placeCommand = new PlaceBuildingCommand(clickedItem.PickedPosition);
                
                var validationResult = _placeBuildingCommandValidator.IsValid(placeCommand);
                if (validationResult)
                {
                    _mediator.Send(placeCommand);
                }
                
                break;
            }
        }
        
    }
}