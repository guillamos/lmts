using System;
using LMTS.Common.Enums;
using LMTS.Common.Abstract;

namespace LMTS.Common.Models.StaticData;

public class PathLane
{
    public PathLane(PathLaneType type, decimal offset, decimal width)
    {
        Type = type;
        Offset = offset;
        Width = width;
    }
    

    public PathLaneType Type { get; set; }

    public decimal Offset { get; set; }

    public decimal Width { get; set; }
}