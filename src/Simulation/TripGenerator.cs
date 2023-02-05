using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.CommonServices.Abstract;
using LMTS.Navigation.NavigationGraphs;
using LMTS.State.WorldState.Abstract;
using MediatR;

namespace LMTS.Simulation;

//todo generate trips more intelligently per person instead of randomly per building
public class TripGenerator
{
    private readonly IWorldStateCollectionStore<WorldBuilding> _buildingCollectionStore;
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;
    private readonly IPathInteractionPointService _pathInteractionPointService;
    private readonly IMediator _mediator;
    private readonly RoadVehicleNavigationGraph _roadVehicleNavigationGraph;

    private Dictionary<Guid, uint> _lastSpawnByBuildingId = new();

    private Random _random = new();

    public TripGenerator(IWorldStateCollectionStore<WorldBuilding> buildingCollectionStore, IPathInteractionPointService pathInteractionPointService, IMediator mediator, RoadVehicleNavigationGraph roadVehicleNavigationGraph, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore)
    {
        _buildingCollectionStore = buildingCollectionStore;
        _pathInteractionPointService = pathInteractionPointService;
        _mediator = mediator;
        _roadVehicleNavigationGraph = roadVehicleNavigationGraph;
        _pathCollectionStore = pathCollectionStore;
    }

    public void PhysicsProcess(int tickRate, uint currentPhysicsTick)
    {
        var spawnEveryXSeconds = 5;
        var spawnIfEarlierThanTick = currentPhysicsTick - tickRate * spawnEveryXSeconds;
        
        foreach (var originBuilding in _buildingCollectionStore.Items)
        {
            var lastSpawn = _lastSpawnByBuildingId.ContainsKey(originBuilding.Identifier)
                ? _lastSpawnByBuildingId[originBuilding.Identifier]
                : 0;

            if (lastSpawn >= spawnIfEarlierThanTick)
            {
                continue;
            }
            
            var randomBuildingIndex = _random.Next(_buildingCollectionStore.Items.Count);
            var destinationBuilding = _buildingCollectionStore.Items[randomBuildingIndex];

            if (destinationBuilding == originBuilding)
            {
                continue;
            }

            var originPathConnectingPoint =
                _pathInteractionPointService.GetClosestInteractionPoint(originBuilding.OriginPosition, 2);
            
            var destinationPathConnectingPoint =
                _pathInteractionPointService.GetClosestInteractionPoint(destinationBuilding.OriginPosition, 2);

            if (originPathConnectingPoint == null || destinationPathConnectingPoint == null)
            {
                //todo handle buildings with no path connection
                throw new Exception("building without path connection");
            }
            
            //todo should be centrally defined
            var validPathLaneTypes = new List<PathLaneType>() { PathLaneType.BasicRoad };
            
            var originClosestPathLane =
                originPathConnectingPoint.Side == PathInteractionPointSide.Left
                    ? originPathConnectingPoint.Path.Lanes.FirstOrDefault(pl =>
                        validPathLaneTypes.Contains(pl.Value.Settings.Type))
                    : originPathConnectingPoint.Path.Lanes.LastOrDefault(pl =>
                        validPathLaneTypes.Contains(pl.Value.Settings.Type));
            
            var destinationClosestPathLane =
                destinationPathConnectingPoint.Side == PathInteractionPointSide.Left
                    ? destinationPathConnectingPoint.Path.Lanes.FirstOrDefault(pl =>
                        validPathLaneTypes.Contains(pl.Value.Settings.Type))
                    : destinationPathConnectingPoint.Path.Lanes.LastOrDefault(pl =>
                        validPathLaneTypes.Contains(pl.Value.Settings.Type));

            //todo these calculations are not yet correct
            var startPoint = originPathConnectingPoint.Position - originPathConnectingPoint.Normal * (float)(originClosestPathLane.Value.Settings.Offset + originClosestPathLane.Value.Settings.Width / 2);
            var destinationPoint = destinationPathConnectingPoint.Position - destinationPathConnectingPoint.Normal * (float)(destinationClosestPathLane.Value.Settings.Offset + destinationClosestPathLane.Value.Settings.Width / 2);

            var itinerary = 
                _roadVehicleNavigationGraph.GetBestRoute(originClosestPathLane.Value.Identifier, destinationClosestPathLane.Value.Identifier);

            var mappedItinerary = MapTripItineraryNodes(itinerary);

            if (mappedItinerary.Count == 0)
            {
                continue;
            }

            mappedItinerary = mappedItinerary.Prepend(new TripItineraryNode(startPoint))
                .Append(new TripItineraryNode(destinationPoint)).ToList();

            var newTripCommand = new AddTripCommand(originBuilding, destinationBuilding,startPoint, destinationPoint, mappedItinerary);

            _mediator.Send(newTripCommand);
                
            _lastSpawnByBuildingId[originBuilding.Identifier] = currentPhysicsTick;
        }
    }

    private List<TripItineraryNode> MapTripItineraryNodes(IEnumerable<(Vector3 point, LaneIdentifier lane)> itinerary)
    {
        var mappedItinerary = new List<TripItineraryNode>();

        foreach (var itineraryNode in itinerary)
        {
            var newItineraryNode = new TripItineraryNode(
                itineraryNode.point,
                itineraryNode.lane
            );

            mappedItinerary.Add(newItineraryNode);
        }

        return mappedItinerary;
    }
}