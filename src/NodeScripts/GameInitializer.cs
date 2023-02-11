using Godot;
using LMTS.CommandSystem.Commands.LocalCommands;
using LMTS.DependencyInjection;
using LMTS.Initialization;
using LMTS.Presentation.Overlay.Enums;
using MediatR;

namespace LMTS.NodeScripts;

public partial class GameInitializer: Node
{
    [Inject] private PathTypeInitializer _pathTypeInitializer;
    [Inject] private IMediator _mediator;
    
    public override void _EnterTree()
    {
        this.ResolveDependencies();
        _pathTypeInitializer.Initialize();
        
    }
    
    public override void _Ready()
    {
        //always activate the tool specific overlay
        var command = new ToggleOverlayCommand(OverlayType.Tool);
        _mediator.Send(command);
    }
}