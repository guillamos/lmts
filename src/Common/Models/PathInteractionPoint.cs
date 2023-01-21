using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;

namespace LMTS.Common.Models;

public record PathInteractionPoint(PathInteractionPointSide Side, int SideLocalIdentifier, Vector3 Position, WorldNavigationPath Path, Vector3 Normal)
{
}