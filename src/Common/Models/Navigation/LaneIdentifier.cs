using System;

namespace LMTS.Common.Models.Navigation;

//todo: should this be a record?
public struct LaneIdentifier
{
    public LaneIdentifier(Guid path, int lane)
    {
        Path = path;
        Lane = lane;
    }

    public Guid Path { get; set; }
    public int Lane { get; set; }
}