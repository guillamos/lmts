using Godot;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class PlaceBuildingCommand : IRequest
{
    public PlaceBuildingCommand(Vector3 centerPosition)
    {
        CenterPosition = centerPosition;
    }
    
    public Vector3 CenterPosition { get; set; }
}