using System;
using Godot;
using LMTS.Common.Models.World;

namespace LMTS.Common.Utilities;

public static class NavigationPathGeometryUtilties
{

    public static Vector3 GetRelativePositionAlongPath(WorldNavigationPath path, float weight, decimal perpendicularOffset = 0)
    {
        //todo implement for curved paths and such

        var centerPoint = path.From.Position.Lerp(path.To.Position, weight);

        if (perpendicularOffset == 0)
        {
            return centerPoint;
        }
        
        var perpendicularDirectionVector = path.From.Position.DirectionTo(path.To.Position).Rotated(Vector3.Up, (float)Math.PI / 2);

        var offsetCenterPoint = centerPoint + (perpendicularDirectionVector * (float)perpendicularOffset);

        return offsetCenterPoint;
    }

    public static Vector3 GetAbsolutePositionAlongPath(WorldNavigationPath path, float distanceFromStart,
        decimal offset = 0)

    {
        throw new NotImplementedException();
    }
}