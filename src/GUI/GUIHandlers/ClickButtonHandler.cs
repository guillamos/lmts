//TODO think of a better way to plug the GUI to the c# backend. Not really into the Godot GUI stuff yet

using System;
using LMTS.CommandSystem.Commands.LocalCommands;
using LMTS.GUI.Abstract;
using LMTS.GUI.Enums;
using LMTS.InputToolSystem.Enums;
using LMTS.Presentation.Overlay.Enums;
using MediatR;

namespace LMTS.GUI.GUIHandlers
{
    public class ClickButtonHandler: IClickButtonHandler
    {
        private readonly IMediator _mediator;

        public ClickButtonHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void HandleButtonAction(ButtonAction action, string actionData)
        {
            IRequest command;
            
            switch (action)
            {
                case ButtonAction.ActivatePlaceRoadTool:
                    command = new ActivateToolCommand(ToolType.PlaceNavigationPath, actionData);
                    break;
                case ButtonAction.ActivatePlaceBuildingTool:
                    command = new ActivateToolCommand(ToolType.PlaceBuilding, actionData);
                    break;
                case ButtonAction.ToggleLaneOverlay:
                    command = new ToggleOverlayCommand(OverlayType.Lanes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            _mediator.Send(command);
        }
    }
}
