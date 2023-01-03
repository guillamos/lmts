using System.Diagnostics;
using System.Linq;
using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.DependencyInjection;
using LMTS.InputHandling;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using MediatR;

namespace LMTS.InputToolSystem.Tools;

public class PlaceNavigationPathTool: IInputTool
{
    [Inject] private readonly IInputManager _inputManager;

    private WorldNavigationJunction _firstClickedJunction;
    private PlaceNavigationPathCommandValidator _placeNavigationPathCommandValidator;
    private IMediator _mediator;

    public PlaceNavigationPathTool(IInputManager inputManager, PlaceNavigationPathCommandValidator placeNavigationPathCommandValidator, IMediator mediator)
    {
        _inputManager = inputManager;
        _placeNavigationPathCommandValidator = placeNavigationPathCommandValidator;
        _mediator = mediator;
    }

    public void Activate()
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

        WorldNavigationJunction clickedJunction = null;

        foreach (var clickedItem in clickedItems)
        {
            //todo check if clicked existing junction
            
            //todo maybe get floor by script or metadata or something
            if (clickedItem.Node.Name == "FloorPlane")
            {
                //create a new junction on this position
                //todo: check if this is a valid position for a junction
                clickedJunction = new WorldNavigationJunction(WorldObjectState.PreviewGhost, clickedItem.PickedPosition);
            }
        }

        if (clickedJunction != null)
        {
            if (_firstClickedJunction == null)
            {
                _firstClickedJunction = clickedJunction;
            }
            else
            {
                var placeCommand = new PlaceNavigationPathCommand(_firstClickedJunction, clickedJunction);

                var validationResult = _placeNavigationPathCommandValidator.IsValid(placeCommand);
                if (validationResult)
                {
                    _mediator.Send(placeCommand);
                    _firstClickedJunction = null;
                }
            }
        }
    }
    
}