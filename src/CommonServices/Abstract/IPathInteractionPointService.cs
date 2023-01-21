using LMTS.Common.Models;

namespace LMTS.CommonServices.Abstract;

public interface IPathInteractionPointService
{
    PathInteractionPoint? GetClosestInteractionPoint(Godot.Vector3 point, float maxDistance);
}