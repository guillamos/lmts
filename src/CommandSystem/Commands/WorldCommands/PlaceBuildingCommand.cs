using System.Collections.Generic;
using Godot;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class PlaceBuildingCommand : IRequest
{
    public PlaceBuildingCommand(Vector3 originPosition, IEnumerable<Vector3> plotPolygon)
    {
        OriginPosition = originPosition;
        PlotPolygon = plotPolygon;
    }
    
    public Vector3 OriginPosition { get; set; }
    
    public IEnumerable<Vector3> PlotPolygon { get; set; }
}