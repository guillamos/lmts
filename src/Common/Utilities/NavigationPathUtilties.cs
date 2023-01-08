using System;
using Godot;
using LMTS.Common.Models.World;

namespace LMTS.Common.Utilities;

public static class NavigationPathUtilties
{

    public static Vector3 GetRelativePositionAlongPath(WorldNavigationPath path, float weight)
    {
        //todo implement for curved paths and such

        return path.From.Position.Lerp(path.To.Position, weight);
    }
    
    public static Vector3 GetAbsolutePositionAlongPath(WorldNavigationPath path, float distanceFromStart)
    {
        throw new NotImplementedException();
    }
}