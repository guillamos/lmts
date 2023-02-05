using Godot;
using LMTS.DependencyInjection;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using LMTS.Simulation;

namespace LMTS.NodeScripts;

public partial class MainSimulationLoop: Node
{
    [Inject] private IToolManager _toolManager;
    [Inject] private readonly IInputManager _inputManager;
    [Inject] private readonly TripGenerator _tripGenerator;
    [Inject] private readonly TripSimulator _tripSimulator;

    private uint _currentPhysicsTick = 0;
    private int _tickRate = 60;

    public override void _Ready()
    {
        this.ResolveDependencies();
        _tripSimulator.SetParentNode(this);
    }
    public override void _PhysicsProcess(double delta)
    {
        //todo might want to process the active tool in the frame render to reduce input lag
        _toolManager.ProcessActiveTool();
        _inputManager.ClearClickInputForTick();
        
        _tripSimulator.PhysicsProcess(_tickRate, _currentPhysicsTick);
        _tripGenerator.PhysicsProcess(_tickRate, _currentPhysicsTick);
        
        _currentPhysicsTick++;
    }
}