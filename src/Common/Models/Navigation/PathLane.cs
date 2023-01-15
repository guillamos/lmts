using System.IO;
using LMTS.Common.Models.StaticData;
using LMTS.Common.Models.World;

namespace LMTS.Common.Models.Navigation;

public class PathLane
{
    public PathLane(WorldNavigationPath path, int laneId, PathLaneSettings settings)
    {
        Path = path;
        Settings = settings;
        Identifier = new LaneIdentifier(path.Identifier, laneId);
    }

    public WorldNavigationPath Path { get; }
    
    public LaneIdentifier Identifier { get; }
    
    public PathLaneSettings Settings { get; }
}