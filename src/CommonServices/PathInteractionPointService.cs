using System;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.Common.Extensions;
using LMTS.Common.Models;
using LMTS.CommonServices.Abstract;
using LMTS.State.WorldState.Abstract;
using Octree;

namespace LMTS.CommonServices;

public class PathInteractionPointService: IPathInteractionPointService
{
    private PointOctree<PathInteractionPoint> _octree = new(1000, Vector3.Zero, 100);

    public PathInteractionPointService(IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore)
    {
        pathCollectionStore.Items.CollectionChanged += PathsChanged;
    }
    
    public PathInteractionPoint? GetClosestInteractionPoint(Godot.Vector3 point, float maxDistance)
    {
        var nearby = _octree.GetNearby(point.ToDotnetVector3(), maxDistance);

        if (!nearby.Any())
        {
            return null;
        }

        var closest = nearby.MinBy(p => p.Position.DistanceTo(point));

        return closest;
    }
    
    private void PathsChanged (object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var path in e.NewItems.OfType<WorldNavigationPath>())
                    {
                        CreateInteractionPointsFromWorldPath(path);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Replace:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Move:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Reset:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CreateInteractionPointsFromWorldPath(WorldNavigationPath path)
    {
        foreach (var point in NavigationPathGeometryUtilities.GetPathInteractionPoints(path))
        {
            _octree.Add(point, point.Position.ToDotnetVector3());
        }
    }
}