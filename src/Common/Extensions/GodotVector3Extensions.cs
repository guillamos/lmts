namespace LMTS.Common.Extensions;

public static class GodotVector3Extensions
{
    public static System.Numerics.Vector3 ToDotnetVector3(this Godot.Vector3 vector3)
    {
        return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
    }
}