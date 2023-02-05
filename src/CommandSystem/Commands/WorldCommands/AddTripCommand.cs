using System.Collections.Generic;
using Godot;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;
using MediatR;

namespace LMTS.CommandSystem.Commands.WorldCommands;

public class AddTripCommand: IRequest
{
    public AddTripCommand(WorldBuilding from, WorldBuilding to, Vector3 fromPosition, Vector3 toPosition, IList<TripItineraryNode> tripItinerary)
    {
        From = from;
        To = to;
        FromPosition = fromPosition;
        ToPosition = toPosition;
        TripItinerary = tripItinerary;
    }

    public WorldBuilding From { get; set; }
        
    public WorldBuilding To { get; set; }
    
    public Vector3 FromPosition { get; set; }
    
    public Vector3 ToPosition { get; set; }
    
    public IList<TripItineraryNode> TripItinerary { get; set; }
}