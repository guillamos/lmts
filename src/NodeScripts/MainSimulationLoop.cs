using Godot;
using LMTS.DependencyInjection;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;

namespace LMTS.NodeScripts;

public partial class MainSimulationLoop: Node
{
    [Inject] private IToolManager _toolManager;
    [Inject] private readonly IInputManager _inputManager;

    public override void _Ready()
    {
        this.ResolveDependencies();
    }
    public override void _PhysicsProcess(double delta)
    {
        //todo might want to process the active tool in the frame render to reduce input lag
        _toolManager.ProcessActiveTool();
        _inputManager.ClearClickInputForTick();
    }
}