namespace LMTS.Common.Extensions;

public static class GodotVector3Extensions
{
    public static System.Numerics.Vector3 ToDotnetVector3(this Godot.Vector3 vector3)
    {
        return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}