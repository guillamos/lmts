using System.Collections.Generic;
using LMTS.Common.Enums;
using LMTS.Common.Models.StaticData;
using LMTS.Navigation;
using LMTS.State.LocalState;

namespace LMTS.Initialization;

public class PathTypeInitializer
{
    private readonly StaticDataStore _staticDataStore;
    private readonly NavigationGraphManager _navigationGraphManager;

    public PathTypeInitializer(StaticDataStore staticDataStore, NavigationGraphManager navigationGraphManager)
    {
        _staticDataStore = staticDataStore;
        _navigationGraphManager = navigationGraphManager;
    }

    public void Initialize()
    {
        // initializes all the path types. In the future this will probably be loaded from configuration files
        // lanes are defined left-to-right
        var pathTypes = new List<PathType>()
        {
            new PathType("BASIC_ROAD", 10,
                new List<PathLane>()
                {
                    new PathLane(PathLaneType.Sidewalk, PathLaneDirection.Bidirectional, -5, 2),
                    new PathLane(PathLaneType.BasicRoad, PathLaneDirection.ToToFrom, -3, 3),
                    new PathLane(PathLaneType.BasicRoad, PathLaneDirection.FromToTo, 0, 3),
                    new PathLane(PathLaneType.Sidewalk, PathLaneDirection.Bidirectional, 3, 2),
                })
        };
        
        _staticDataStore.Initialize(pathTypes);
    }
}