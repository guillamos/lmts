using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;

namespace LMTS.Common.Models;

public class PathInteractionPoint
{
    public PathInteractionPointType Type;
    public int LocalIdentifier;
    public Vector3 Position;
    public WorldNavigationPath Path;
    public Vector3 Normal;

    public PathInteractionPoint(PathInteractionPointType type, int localIdentifier, Vector3 position, WorldNavigationPath path, Vector3 normal)
    {
        Type = type;
        LocalIdentifier = localIdentifier;
        Position = position;
        Path = path;
        Normal = normal;
    }
}