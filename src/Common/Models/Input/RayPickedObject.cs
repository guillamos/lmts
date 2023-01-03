using System;
using Godot;

namespace LMTS.Common.Models.Input;

public class RayPickedObject
{
    public RayPickedObject(CollisionObject3D node, Vector3 pickedPosition)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        PickedPosition = pickedPosition;
    }

    public CollisionObject3D Node { get; set; }
    
    public Vector3 PickedPosition { get; set; }
}