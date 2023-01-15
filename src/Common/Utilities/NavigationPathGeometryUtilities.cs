using System;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;

namespace LMTS.Common.Utilities;

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
}