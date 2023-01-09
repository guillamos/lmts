using System;
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
    public WorldBuilding(WorldObjectState state, Vector3 position) : base(state)
    {
        Position = position;
    }
    
    public Vector3 Position { get; set; }

}