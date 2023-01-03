using System;
using LMTS.Common.Models.World;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class PlaceNavigationPathCommand : IRequest
{
    public PlaceNavigationPathCommand(WorldNavigationJunction fromJunction, WorldNavigationJunction toJunction)
    {
        FromJunction = fromJunction ?? throw new ArgumentNullException(nameof(fromJunction));
        ToJunction = toJunction ?? throw new ArgumentNullException(nameof(toJunction));
    }

    public WorldNavigationJunction FromJunction { get; set; }
    
    public WorldNavigationJunction ToJunction { get; set; }
}