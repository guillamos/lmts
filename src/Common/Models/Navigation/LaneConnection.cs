using System;
using LMTS.Common.Models.World;

namespace LMTS.Common.Models.Navigation;

public class LaneConnection
{
    public LaneConnection(PathLane laneFrom, PathLane laneTo, WorldNavigationJunction junction)
    {
        LaneFrom = laneFrom;
        LaneTo = laneTo;
        Junction = junction;
    }

    public PathLane LaneFrom { get; set; }
    public PathLane LaneTo { get; set; }
    public WorldNavigationJunction Junction { get; set; }
}