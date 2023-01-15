using System;
using LMTS.Common.Enums;
using LMTS.Common.Abstract;

namespace LMTS.Common.Models.StaticData;

public class PathLaneSettings
{
    public PathLaneSettings(PathLaneType type, PathLaneDirection direction, decimal offset, decimal width)
    {
        Type = type;
        Direction = direction;
        Offset = offset;
        Width = width;
    }
    

    public PathLaneType Type { get; set; }
    
    public PathLaneDirection Direction { get; set; }

    public decimal Offset { get; set; }

    public decimal Width { get; set; }
}