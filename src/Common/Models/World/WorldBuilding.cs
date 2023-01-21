using System;
using System.Collections.Generic;
using Godot;
using LMTS.Common.Abstract;
using LMTS.Common.Enums;
using LMTS.Common.Models.StaticData;

namespace LMTS.Common.Models.World;

/// <summary>
/// A navigation path in the world between two junctions, which is used for navigation of entities and might be rendered
/// </summary>
public class WorldBuilding: BaseWorldObject
{
    public WorldBuilding(WorldObjectState state, Vector3 originPosition, IEnumerable<Vector3> plotPolygon) : base(state)
    {
        OriginPosition = originPosition;
        PlotPolygon = plotPolygon;
    }
    
    public Vector3 OriginPosition { get; set; }
    
    public IEnumerable<Vector3> PlotPolygon { get; set; }

}