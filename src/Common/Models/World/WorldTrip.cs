using System.Collections.Generic;
using Godot;
using LMTS.Common.Abstract;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;

namespace LMTS.Common.Models.World;

public class WorldTrip : BaseWorldObject
{
    public WorldTrip(WorldObjectState state, WorldBuilding from, WorldBuilding to, Vector3 fromPosition, Vector3 toPosition, IEnumerable<TripItineraryNode> tripItinerary) : base(state)
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
    
    public IEnumerable<TripItineraryNode> TripItinerary { get; set; }
}