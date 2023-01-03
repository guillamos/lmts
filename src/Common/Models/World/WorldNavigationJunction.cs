using System;
using System.Collections.Generic;
using Godot;
using LMTS.Common.Abstract;
using LMTS.Common.Enums;

namespace LMTS.Common.Models.World;

/// <summary>
/// A navigation junction in the world which connects multiple paths
/// </summary>
public class WorldNavigationJunction: BaseWorldObject
{
    public WorldNavigationJunction(WorldObjectState state, Vector3 position) : base(state)
    {
        Position = position;
    }

    public Vector3 Position { get; }
    
    public List<WorldNavigationPath> LinkedPaths { get; } = new();
}