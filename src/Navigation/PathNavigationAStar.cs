using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;

namespace LMTS.Navigation;

//todo split to specific subtypes per navigation graph type?
public class PathNavigationAStar
{
    private readonly AStar3D _aStarGraph;
    private readonly IEnumerable<PathLaneType> _validLaneTypes;
    //todo: should this be 2 nested dictionaries instead of a tuple? investigate
    private readonly IDictionary<(Guid path, int lane), long> _pathLaneIds = new Dictionary<(Guid path, int lane), long>();
    private long _newIdCounter = 0;

    public PathNavigationAStar(IEnumerable<PathLaneType> validLaneTypes)
    {
        _validLaneTypes = validLaneTypes;
        _aStarGraph = new AStar3D();
    }

    public void AddNewPath(WorldNavigationPath path)
    {
        var validLanes = path.PathType.Lanes.Where(lane => _validLaneTypes.Contains(lane.Type)).ToList();

        var pathConnections =
            new List<(PathLaneDirection currentPathLinkPoint, bool isSameDirection, WorldNavigationPath
                linkedPath)>();
        
        pathConnections.AddRange(path.From.LinkedPaths
            .Where(lp => lp.Identifier != path.Identifier)
            .Select(lp => (PathLaneDirection.FromToTo, lp.To == path.From, lp)));
        pathConnections.AddRange(path.To.LinkedPaths
            .Where(lp => lp.Identifier != path.Identifier)
            .Select(lp => (PathLaneDirection.ToToFrom, lp.From == path.To, lp)));

        //loop through all the lanes of the new path
        foreach (var lane in validLanes.Select((item, idx) => (idx, item)))
        {
            AddLane(path, lane.idx);
            
            //loop through all the paths which connect the current path
            foreach (var connection in pathConnections)
            {
                var validLinkedLanes = connection.linkedPath.Lanes.Where(l => _validLaneTypes.Contains(l.Value.Type));
                //loop through all the lanes which are of a compatible type
                foreach (var linkedLane in validLinkedLanes)
                {
                    if (linkedLane.Value.Direction == PathLaneDirection.Bidirectional &&
                        lane.item.Direction == PathLaneDirection.Bidirectional)
                    {
                        ConnectLanes(connection.linkedPath, linkedLane.Key, path, lane.idx, true);
                    }
                    //todo handle bidirectional lanes
                    //current lane goes from -> to, which means the linked lane has to go the same way if the path connection is equal direction or should go the opposite way if the path connection is not equal direction
                    else if (lane.item.Direction == PathLaneDirection.FromToTo && (
                        (connection.isSameDirection && linkedLane.Value.Direction == PathLaneDirection.FromToTo) || 
                        (!connection.isSameDirection && linkedLane.Value.Direction == PathLaneDirection.ToToFrom)))
                    {
                        ConnectLanes(connection.linkedPath, linkedLane.Key, path, lane.idx, false);
                    }
                    else if (lane.item.Direction == PathLaneDirection.ToToFrom && (                            
                        (connection.isSameDirection && linkedLane.Value.Direction == PathLaneDirection.ToToFrom) || 
                        (!connection.isSameDirection && linkedLane.Value.Direction == PathLaneDirection.FromToTo)))
                    {
                        ConnectLanes(path, lane.idx, connection.linkedPath, linkedLane.Key, false);
                    }
                }
            }
        }

        //connect other direction lanes in the same path to allow u-turns
        //todo: only allow u turns when there are no connections and automatically remove these when necessary
        foreach (var lane in validLanes.Select((item, idx) => (idx, item)))
        {
            var samePathOtherDirectionLanes = validLanes.Where(l => l.Direction != lane.item.Direction);
            
            foreach (var otherDirectionLane in samePathOtherDirectionLanes.Select((item, idx) => (idx, item)))
            {
                ConnectLanes(path, lane.idx, path, otherDirectionLane.idx, false);
            }
        }
    }

    private void AddLane(WorldNavigationPath path, int laneNumber)
    {
        //todo calculate the midpoint per lane
        var pathMidpoint = NavigationPathGeometryUtilties.GetRelativePositionAlongPath(path, 0.5f);
        var pathLength = path.From.Position.DistanceTo(path.To.Position);
        
        var newLaneId = _newIdCounter++;
        
        _pathLaneIds.Add((path.Identifier, laneNumber), newLaneId);

        //todo calculate weight based on factors like lane max speed etc instead of only distance
        _aStarGraph.AddPoint(newLaneId, pathMidpoint, pathLength);
    }

    private void ConnectLanes(WorldNavigationPath pathFrom, int laneIdxFrom, WorldNavigationPath pathTo, int laneIdxTo, bool isBidirectional)
    {
        var laneId1 = _pathLaneIds[(pathFrom.Identifier, laneIdxFrom)];
        var laneId2 = _pathLaneIds[(pathTo.Identifier, laneIdxTo)];

        _aStarGraph.ConnectPoints(laneId1, laneId2, isBidirectional);
    }
}