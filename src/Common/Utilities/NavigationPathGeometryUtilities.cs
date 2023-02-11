using System;
using System.Collections.Generic;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;

namespace LMTS.Common.Utilities;

//todo handle bezier paths for all methods
public static class NavigationPathGeometryUtilities
{
    public static Vector3 GetRelativePositionAlongPath(WorldNavigationPath path, float weight, decimal perpendicularOffset = 0)
    {
        //todo: correction for junctions, should be absolute
        var correctedFromPoint = LerpAbsolute(path.From.Position,path.To.Position, 5);
        var correctedToPoint = LerpAbsolute(path.To.Position, path.From.Position, 5);
        
        //todo implement for curved paths and such

        var centerPoint = correctedFromPoint.Lerp(correctedToPoint, weight);

        if (perpendicularOffset == 0)
        {
            return centerPoint;
        }
        
        var perpendicularDirectionVector = correctedFromPoint.DirectionTo(path.To.Position).Rotated(Vector3.Up, (float)Math.PI / 2);

        var offsetCenterPoint = centerPoint + (perpendicularDirectionVector * (float)perpendicularOffset);

        return offsetCenterPoint;
    }

    public static Vector3 GetAbsolutePositionAlongPath(WorldNavigationPath path, float distanceFromStart, decimal offset = 0)
    {
        throw new NotImplementedException();
    }

    private static Vector3 LerpAbsolute(Vector3 from, Vector3 to, float units)
    {
        var distance = from.DistanceTo(to);
        if (units > distance || units < 0)
        {
            throw new ArgumentException();
        }

        var fraction = units / distance;
        return from.Lerp(to, fraction);
    }

    public static Vector3 GetLaneJunctionConnectionPoint(PathLane lane, WorldNavigationJunction junction)
    {
        var laneMiddle = GetLaneMiddleOffset(lane);
        
        if (lane.Path.To == junction && lane.Settings.Direction is PathLaneDirection.FromToTo or PathLaneDirection.Bidirectional)
        {
            return GetRelativePositionAlongPath(lane.Path, 1f, laneMiddle);
        }
        if (lane.Path.To == junction && lane.Settings.Direction is PathLaneDirection.ToToFrom)
        {
            return GetRelativePositionAlongPath(lane.Path, 1f, laneMiddle);
        }

        if (lane.Path.From == junction && lane.Settings.Direction is PathLaneDirection.ToToFrom or PathLaneDirection.Bidirectional)
        {
            return GetRelativePositionAlongPath(lane.Path, 0f, laneMiddle);
        }
        if (lane.Path.From == junction && lane.Settings.Direction is PathLaneDirection.FromToTo)
        {
            return GetRelativePositionAlongPath(lane.Path, 0f, laneMiddle);
        }
        

        throw new ArgumentException();
    }
    
    public static Vector3 GetLaneRelativePosition(PathLane lane, float relativePosition, bool correctForDirection)
    {
        var laneMiddle = GetLaneMiddleOffset(lane);

        if (!correctForDirection)
        {
            return GetRelativePositionAlongPath(lane.Path, relativePosition, laneMiddle);
        }
        
        if (lane.Settings.Direction is PathLaneDirection.FromToTo or PathLaneDirection.Bidirectional)
        {
            return GetRelativePositionAlongPath(lane.Path, relativePosition, laneMiddle);
        }
        if (lane.Settings.Direction is PathLaneDirection.ToToFrom)
        {
            return GetRelativePositionAlongPath(lane.Path, 1f - relativePosition, laneMiddle);
        }

        throw new ArgumentException();
    }
    
    //todo: precalculate?
    public static decimal GetLaneLeftOffset(PathLane lane)
    {
        return lane.Settings.Offset;
    }
    
    //todo: precalculate?
    public static decimal GetLaneMiddleOffset(PathLane lane)
    {
        return lane.Settings.Offset + (lane.Settings.Width / 2);
    }
    
    //todo: precalculate?
    public static decimal GetLaneRightOffset(PathLane lane)
    {
        return lane.Settings.Offset + lane.Settings.Width;
    }

    //todo: this method should take junction geometry into account
    //todo: precalculate?
    //todo handle curved paths
    public static IEnumerable<PathInteractionPoint> GetPathInteractionPoints(WorldNavigationPath path)
    {
        var offsetFromPointL = GetRelativePositionAlongPath(path, 0, 0 - path.PathType.Width / 2);
        var offsetToPointL = GetRelativePositionAlongPath(path, 1, 0 - path.PathType.Width / 2);
        
        var offsetFromPointR = GetRelativePositionAlongPath(path, 0, path.PathType.Width / 2);
        var offsetToPointR = GetRelativePositionAlongPath(path, 1, path.PathType.Width / 2);

        var distanceBetweenPoints = 2;

        var counter = 0;
        
        //todo calculate normals per point on spline for curved paths
        var leftNormal = offsetFromPointL.DirectionTo(offsetToPointL).Rotated(Vector3.Up, (float)Math.PI / 2);

        foreach (var point in GetSpacedPointsOnSpline(offsetFromPointL, offsetToPointL, distanceBetweenPoints))
        {
            yield return new PathInteractionPoint(PathInteractionPointSide.Left, counter++, point, path, leftNormal);
        }
        
        var rightNormal = offsetFromPointL.DirectionTo(offsetToPointL).Rotated(Vector3.Up, (float)Math.PI / 2);
        
        counter = 0;
        
        foreach (var point in GetSpacedPointsOnSpline(offsetFromPointR, offsetToPointR, distanceBetweenPoints))
        {
            yield return new PathInteractionPoint(PathInteractionPointSide.Right, counter++, point, path, rightNormal);
        }
    }

    private static IEnumerable<Vector3> GetSpacedPointsOnSpline(Vector3 fromPoint, Vector3 toPoint, float distancePerPoint)
    {
        var totalDistance = fromPoint.DistanceTo(toPoint);

        var fraction = distancePerPoint / totalDistance;

        for (int i = 0; i * fraction < 1; i++)
        {
            yield return fromPoint.Lerp(toPoint, i * fraction);
        }
    }
}