using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.Navigation.Abstract;

namespace LMTS.Navigation;

public abstract class PathNavigationGraph: INavigationGraph
{
    private readonly AStar3D _aStarGraph;
    private readonly IEnumerable<PathLaneType> _validLaneTypes;
    private readonly IDictionary<(LaneIdentifier lane, int pointIndex), long> _pathLaneIds = new Dictionary<(LaneIdentifier lane, int pointIndex), long>();
    private readonly IDictionary<long, (LaneIdentifier lane, int pointIndex)> _pathLaneIdsByGraphId = new Dictionary<long, (LaneIdentifier lane, int pointIndex)>();
    private long _newIdCounter = 0;

    public ObservableCollection<LaneConnection> LaneConnections { get; } = new();

    protected PathNavigationGraph(IEnumerable<PathLaneType> validLaneTypes)
    {
        _validLaneTypes = validLaneTypes;
        _aStarGraph = new AStar3D();
    }

    //todo return TripItineraryNodes?
    public IEnumerable<(Vector3 position, LaneIdentifier lane)> GetBestRoute(LaneIdentifier from, LaneIdentifier to)
    {
        var graphFrom = _pathLaneIds[(from, 1)];
        var graphTo = _pathLaneIds[(to, 0)];

        var route = _aStarGraph.GetIdPath(graphFrom, graphTo);

        foreach (var routeNode in route)
        {
            var position = _aStarGraph.GetPointPosition(routeNode);
            yield return (position, _pathLaneIdsByGraphId[routeNode].lane);
        }
    }
    
    public void AddNewPath(WorldNavigationPath path)
    {
        var validLanes = path.Lanes.Where(lane => _validLaneTypes.Contains(lane.Value.Settings.Type)).Select(p => p.Value).ToList();
        
        var pathConnections = new List<WorldNavigationPath>();

        pathConnections.AddRange(path.To.LinkedPaths.Where(lp => lp.Identifier != path.Identifier));
        pathConnections.AddRange(path.From.LinkedPaths.Where(lp => lp.Identifier != path.Identifier));

        //loop through all the lanes of the new path
        foreach (var lane in validLanes)
        {
            var laneFromJunction = GetLaneFromJunction(lane);
            var laneToJunction = GetLaneToJunction(lane);
            
            AddLane(lane);
            
            //loop through all the paths which connect the current path
            foreach (var connection in pathConnections)
            {
                var validLinkedLanes = connection.Lanes.Where(l => _validLaneTypes.Contains(l.Value.Settings.Type));
                //loop through all the lanes which are of a compatible type
                foreach (var linkedLane in validLinkedLanes)
                {
                    //todo handle bidirectional lanes

                    var linkedLaneFromJunction = GetLaneFromJunction(linkedLane.Value);
                    var linkedLaneToJunction = GetLaneToJunction(linkedLane.Value);

                    //todo handle bidirectional lanes
                    if (laneToJunction == linkedLaneFromJunction)
                    {
                        ConnectLanes(lane, linkedLane.Value, false, laneToJunction);
                    }

                    if (laneFromJunction == linkedLaneToJunction)
                    {
                        ConnectLanes(linkedLane.Value, lane, false, laneFromJunction);
                    }
                }
            }
        }

        //connect other direction lanes in the same path to allow u-turns
        //todo: only allow u turns when there are no connections and automatically remove these when necessary?
        //todo also fix for bidirectional lanes
        foreach (var lane in validLanes.Select((item, idx) => (idx, item)))
        {
            var samePathOtherDirectionLanes = validLanes.Where(l => l.Settings.Direction != lane.item.Settings.Direction);
            
            foreach (var otherDirectionLane in samePathOtherDirectionLanes.Select((item, idx) => (idx, item)))
            {
                var junction = lane.item.Settings.Direction == PathLaneDirection.FromToTo ? path.From : path.To;
                ConnectLanes(lane.item, otherDirectionLane.item, false, junction);
            }
        }
    }

    private void AddLane(PathLane lane)
    {
        var pathStart = lane.Settings.Direction == PathLaneDirection.FromToTo 
            ? NavigationPathGeometryUtilities.GetLaneRelativePosition(lane, 0f, false) 
            : NavigationPathGeometryUtilities.GetLaneRelativePosition(lane, 1f, false);
        var pathEnd = lane.Settings.Direction == PathLaneDirection.FromToTo 
            ? NavigationPathGeometryUtilities.GetLaneRelativePosition(lane, 1f, false) 
            : NavigationPathGeometryUtilities.GetLaneRelativePosition(lane, 0f, false);
        //todo calculate weight based on factors like lane max speed etc instead of only distance
        
        var newLaneId1 = _newIdCounter++;
        var laneIdentifier = new LaneIdentifier(lane.Path.Identifier, lane.Identifier.Lane);
        _pathLaneIds.Add((laneIdentifier, 0), newLaneId1);
        _pathLaneIdsByGraphId.Add(newLaneId1, (laneIdentifier, 0));
        _aStarGraph.AddPoint(newLaneId1, pathStart);
        
        var newLaneId2 = _newIdCounter++;
        laneIdentifier = new LaneIdentifier(lane.Path.Identifier, lane.Identifier.Lane);
        _pathLaneIds.Add((laneIdentifier, 1), newLaneId2);
        _pathLaneIdsByGraphId.Add(newLaneId2, (laneIdentifier, 1));
        _aStarGraph.AddPoint(newLaneId2, pathEnd);
        
        _aStarGraph.ConnectPoints(newLaneId1, newLaneId2, lane.Settings.Direction == PathLaneDirection.Bidirectional);
    }

    private void ConnectLanes(PathLane from, PathLane to, bool isBidirectional, WorldNavigationJunction viaJunction)
    {
        var laneId1 = from.Identifier;
        var laneId2 = to.Identifier;
        
        var aStarLaneId1 = _pathLaneIds[(laneId1, 1)];
        var aStarLaneId2 = _pathLaneIds[(laneId2, 0)];

        _aStarGraph.ConnectPoints(aStarLaneId1, aStarLaneId2, isBidirectional);
        LaneConnections.Add(new LaneConnection(from, to, viaJunction));
    }

    private WorldNavigationJunction GetLaneToJunction(PathLane lane)
    {
        if (lane.Settings.Direction == PathLaneDirection.FromToTo)
        {
            return lane.Path.To;
        }

        if(lane.Settings.Direction == PathLaneDirection.ToToFrom)
        {
            return lane.Path.From;
        }

        return null;
    }
    
    private WorldNavigationJunction GetLaneFromJunction(PathLane lane)
    {
        if (lane.Settings.Direction == PathLaneDirection.FromToTo)
        {
            return lane.Path.From;
        }

        if(lane.Settings.Direction == PathLaneDirection.ToToFrom)
        {
            return lane.Path.To;
        }

        return null;
    }
}