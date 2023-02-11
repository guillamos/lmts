using Godot;

namespace LMTS.Common.Models.Navigation;

public class TripItineraryNode
{
    public TripItineraryNode(Vector3 position)
    {
        Position = position;
    }
    public TripItineraryNode(Vector3 position, LaneIdentifier lane)
    {
        Position = position;
        Lane = lane;
    }

    public Vector3 Position { get; }
    public LaneIdentifier? Lane { get; }
}