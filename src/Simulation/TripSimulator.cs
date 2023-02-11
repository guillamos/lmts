using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.Simulation.Models;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Simulation;

public class TripSimulator
{
    private IWorldStateCollectionStore<WorldTrip> _worldTripCollectionStore;

    private List<SimulatedTrip> _simulatedTrips = new();

    private Node _parentNode;
    private Mesh _mesh;

    public TripSimulator(IWorldStateCollectionStore<WorldTrip> worldTripCollectionStore)
    {
        _worldTripCollectionStore = worldTripCollectionStore;

        _worldTripCollectionStore.Items.CollectionChanged += TripsChanged;
        
        _mesh = ResourceLoader.Load<Mesh>("res://assets/mesh/testentitymesh.tres");
    }

    public void SetParentNode(Node node)
    {
        _parentNode = node;
    }
    
    private void TripsChanged (object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    //todo: ignore non-final items
                    foreach (var trip in e.NewItems.OfType<WorldTrip>())
                    {
                        AddTrip(trip);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Replace:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Move:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Reset:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AddTrip(WorldTrip worldTrip)
    {
        var meshInstance = new MeshInstance3D();
        
        
        
        _parentNode.AddChild(meshInstance);
        
        meshInstance.Mesh = _mesh;
        meshInstance.Position = worldTrip.TripItinerary.First().Position;
        meshInstance.Rotation = Vector3.Up;
        meshInstance.Scale = new Vector3(2, 2, 2);

        _simulatedTrips.Add(new SimulatedTrip(
            worldTrip,
            worldTrip.TripItinerary.First().Position,
            0,
            worldTrip.TripItinerary.Skip(1).First().Position,
            worldTrip.TripItinerary.First().Lane,
            meshInstance,
            1
        ));
    }

    public void PhysicsProcess(int tickRate, uint currentPhysicsTick)
    {
        var delta = (float)60 / tickRate;
        
        for (int i = _simulatedTrips.Count - 1; i >= 0; i--)
        {
            var trip = _simulatedTrips[i];

            
            //just accelerate until we have information about other entities and maximum path speeds
            //todo add braking conditions
            trip.CurrentSpeed = Math.Min(1f * delta, trip.CurrentSpeed + 0.001f * delta);

            var carryoverSpeed = trip.CurrentSpeed;

            while (carryoverSpeed > 0)
            {
                var distance = trip.CurrentPosition.DistanceTo(trip.CurrentGoalPosition);

                if (carryoverSpeed >= distance)
                {
                    trip.CurrentPosition = trip.CurrentGoalPosition;
                    carryoverSpeed -= distance;

                    trip.CurrentGoalIndex += 1;

                    if (trip.CurrentGoalIndex == trip.WorldTripReference.TripItinerary.Count)
                    {
                        trip.MeshInstance.QueueFree();
                        carryoverSpeed = 0;
                        _simulatedTrips.RemoveAt(i);
                        continue;
                    }

                    var newGoal = trip.WorldTripReference.TripItinerary[trip.CurrentGoalIndex];

                    trip.CurrentGoalPosition = newGoal.Position;
                    trip.CurrentLane = newGoal.Lane;
                }
                else
                {
                    trip.CurrentPosition = trip.CurrentPosition.MoveToward(trip.CurrentGoalPosition, carryoverSpeed);
                    carryoverSpeed -= distance;
                }
                
                trip.MeshInstance.Position = trip.CurrentPosition + new Vector3(0, 0.2f, 0);
            }
        }
    }
}