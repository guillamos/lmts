using Godot;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;

namespace LMTS.Simulation.Models;

public class SimulatedTrip
{
    public SimulatedTrip(WorldTrip worldTripReference, Vector3 currentPosition, float currentSpeed,
        Vector3 currentGoalPosition, LaneIdentifier? currentLane, MeshInstance3D meshInstance, int currentGoalIndex)
    {
        WorldTripReference = worldTripReference;
        CurrentPosition = currentPosition;
        CurrentSpeed = currentSpeed;
        CurrentGoalPosition = currentGoalPosition;
        CurrentLane = currentLane;
        MeshInstance = meshInstance;
        CurrentGoalIndex = currentGoalIndex;
    }
    
    public WorldTrip WorldTripReference { get; set; }
    public Vector3 CurrentPosition { get; set; }
    public float CurrentSpeed { get; set; }
    public Vector3 CurrentGoalPosition { get; set; }
    public LaneIdentifier? CurrentLane { get; set; }
    
    public int CurrentGoalIndex { get; set; }
    public MeshInstance3D MeshInstance { get; set; }
}