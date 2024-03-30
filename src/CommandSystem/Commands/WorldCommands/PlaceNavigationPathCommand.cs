using System;
using LMTS.Common.Models.StaticData;
using LMTS.Common.Models.World;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class PlaceNavigationPathCommand : IRequest
{
    public PlaceNavigationPathCommand(Guid fromJunction, Guid toJunction, PathType pathType)
    {
        FromJunction = fromJunction;
        ToJunction = toJunction;
        PathType = pathType;
    }

    public Guid FromJunction { get; set; }
    
    public Guid ToJunction { get; set; }
    
    public PathType PathType { get; set; }
}