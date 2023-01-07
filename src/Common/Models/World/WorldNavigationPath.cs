using System;
using LMTS.Common.Abstract;
using LMTS.Common.Enums;
using LMTS.Common.Models.StaticData;

namespace LMTS.Common.Models.World;

/// <summary>
/// A navigation path in the world between two junctions, which is used for navigation of entities and might be rendered
/// </summary>
public class WorldNavigationPath: BaseWorldObject
{
    public WorldNavigationPath(WorldObjectState state, WorldNavigationJunction from, WorldNavigationJunction to, PathType pathType) : base(state)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));
        To = to ?? throw new ArgumentNullException(nameof(to));
        PathType = pathType;
    }
    public WorldNavigationJunction From { get; }
    public WorldNavigationJunction To { get; }
    public PathType PathType { get; }
}