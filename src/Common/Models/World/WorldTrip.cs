using System.Collections.Generic;
using Godot;
using LMTS.Common.Abstract;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;

namespace LMTS.Common.Models.World;

public class WorldTrip : BaseWorldObject
{
    public WorldTrip(WorldObjectState state, WorldBuilding from, WorldBuilding to, IList<TripItineraryNode> tripItinerary) : base(state)
    {
        From = from;
        To = to;
        TripItinerary = tripItinerary;
    }

    public WorldBuilding From { get; set; }
        
    public WorldBuilding To { get; set; }

    public IList<TripItineraryNode> TripItinerary { get; set; }
}