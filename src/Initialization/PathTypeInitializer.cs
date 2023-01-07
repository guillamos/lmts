using System.Collections.Generic;
using LMTS.Common.Enums;
using LMTS.Common.Models.StaticData;
using LMTS.State.LocalState;

namespace LMTS.Initialization;

public class PathTypeInitializer
{
    private readonly StaticDataStore _staticDataStore;

    public PathTypeInitializer(StaticDataStore staticDataStore)
    {
        _staticDataStore = staticDataStore;
    }

    public void Initialize()
    {
        var pathTypes = new List<PathType>()
        {
            new PathType("BASIC_ROAD", 10,
                new List<PathLane>()
                {
                    new PathLane(PathLaneType.Sidewalk, -5, 2),
                    new PathLane(PathLaneType.BasicRoad, -3, 3),
                    new PathLane(PathLaneType.BasicRoad, 0, 3),
                    new PathLane(PathLaneType.Sidewalk, 3, 2),
                })
        };
        
        _staticDataStore.Initialize(pathTypes);
    }
}