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
        //todo calculate the midpoint per lane
        var pathMidpoint = NavigationPathUtilties.GetRelativePositionAlongPath(path, 0.5f);

        //todo think of how to handle other road types that can handle road vehicles
        var validLanes = path.PathType.Lanes.Where(lane => _validLaneTypes.Contains(lane.Type)).ToList();

        var pathLength = path.From.Position.DistanceTo(path.To.Position);
        
        foreach (var lane in validLanes.Select((item, idx) => (idx, item)))
        {
            var newLaneId = _newIdCounter++;
            
            _pathLaneIds.Add((path.Identifier, lane.idx), newLaneId);
            
            //todo calculate weight based on factors like lane max speed etc instead of only distance
            _aStarGraph.AddPoint(newLaneId, pathMidpoint, pathLength);
            
            IEnumerable<WorldNavigationPath> allConnectedPathsFrom = new List<WorldNavigationPath>();
            IEnumerable<WorldNavigationPath> allConnectedPathsTo = new List<WorldNavigationPath>();
            
            if (lane.item.Direction == PathLaneDirection.FromToTo)
            {
                allConnectedPathsFrom = path.From.LinkedPaths.Where(lp => lp.Identifier != path.Identifier);
                allConnectedPathsTo = path.To.LinkedPaths.Where(lp => lp.Identifier != path.Identifier);
            }
            else if (lane.item.Direction == PathLaneDirection.ToToFrom)
            {
                allConnectedPathsFrom = path.To.LinkedPaths.Where(lp => lp.Identifier != path.Identifier);
                allConnectedPathsTo = path.From.LinkedPaths.Where(lp => lp.Identifier != path.Identifier);
            }

            foreach (var fromPath in allConnectedPathsFrom)
            {
                //todo handle bidirectional? are bidirectional roads even a thing?
                var validFromLanes = fromPath.PathType.Lanes.Where(l => _validLaneTypes.Contains(l.Type) && l.Direction == lane.item.Direction);
                foreach (var fromLane in validFromLanes.Select((item, idx) => (idx, item)))
                {
                    //todo: should we first create a point for the junction to account for things like traffic lights and more accurate estimates?
                    var isBidirectional = lane.item.Direction == PathLaneDirection.Bidirectional ||
                                          fromLane.item.Direction == PathLaneDirection.Bidirectional;
                    ConnectLanes(fromPath, fromLane.idx, path, lane.idx, isBidirectional);
                }
            }
            
            foreach (var toPath in allConnectedPathsTo)
            {
                //todo handle bidirectional? are bidirectional roads even a thing?
                var validToLanes = toPath.PathType.Lanes.Where(l => _validLaneTypes.Contains(l.Type) && l.Direction == lane.item.Direction);
                foreach (var toLane in validToLanes.Select((item, idx) => (idx, item)))
                {
                    //todo: should we first create a point for the junction to account for things like traffic lights and more accurate estimates?
                    var isBidirectional = lane.item.Direction == PathLaneDirection.Bidirectional ||
                                           toLane.item.Direction == PathLaneDirection.Bidirectional;
                    ConnectLanes(path, lane.idx, toPath, toLane.idx, isBidirectional);
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

    private void ConnectLanes(WorldNavigationPath pathFrom, int laneIdxFrom, WorldNavigationPath pathTo, int laneIdxTo, bool isBidirectional)
    {
        var laneId1 = _pathLaneIds[(pathFrom.Identifier, laneIdxFrom)];
        var laneId2 = _pathLaneIds[(pathTo.Identifier, laneIdxTo)];

        _aStarGraph.ConnectPoints(laneId1, laneId2, isBidirectional);
    }
}