namespace LMTS.Common.Extensions;

public static class DotnetVector3Extensions
{
    public static Godot.Vector3 ToGodotVector3(this System.Numerics.Vector3 vector3)
    {
        return new Godot.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}