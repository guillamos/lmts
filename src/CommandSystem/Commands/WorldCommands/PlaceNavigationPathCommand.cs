using System;
using LMTS.Common.Models.StaticData;
using LMTS.Common.Models.World;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class PlaceNavigationPathCommand : IRequest
{
    public PlaceNavigationPathCommand(WorldNavigationJunction fromJunction, WorldNavigationJunction toJunction, PathType pathType)
    {
        FromJunction = fromJunction ?? throw new ArgumentNullException(nameof(fromJunction));
        ToJunction = toJunction ?? throw new ArgumentNullException(nameof(toJunction));
        PathType = pathType;
    }

    public WorldNavigationJunction FromJunction { get; set; }
    
    public WorldNavigationJunction ToJunction { get; set; }
    
    public PathType PathType { get; set; }
}