//TODO think of a better way to plug the GUI to the c# backend. Not really into the Godot GUI stuff yet

using System;
using LMTS.CommandSystem.Commands.LocalCommands;
using LMTS.GUI.Abstract;
using LMTS.GUI.Enums;
using LMTS.InputToolSystem.Enums;
using LMTS.InputToolSystem.Tools;
using MediatR;

namespace LMTS.GUI.GUIHandlers
{
    public class ClickButtonHandler: IClickButtonHandler
    {
        private readonly PlaceNavigationPathTool _placeNavigationPathTool;
        private readonly IMediator _mediator;

        public ClickButtonHandler(PlaceNavigationPathTool placeNavigationPathTool, IMediator mediator)
        {
            _placeNavigationPathTool = placeNavigationPathTool;
            _mediator = mediator;
        }

        public void HandleButtonAction(ButtonAction action)
        {
            IRequest command;
            
            switch (action)
            {
                case ButtonAction.ActivatePlaceRoadTool:
                    command = new ActivateToolCommand(ToolType.PlaceNavigationPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            _mediator.Send(command);
        }
    }
}
