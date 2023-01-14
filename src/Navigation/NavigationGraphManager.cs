using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Navigation;

public class NavigationGraphManager
{
    private PathNavigationAStar _roadGraph;
    private PathNavigationAStar _pedestrianGraph;
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    public NavigationGraphManager(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
        _roadGraph = new PathNavigationAStar(new List<PathLaneType>() { PathLaneType.BasicRoad });
        _pedestrianGraph = new PathNavigationAStar(new List<PathLaneType>() { PathLaneType.Sidewalk });
        _pathCollectionStore.Items.CollectionChanged += PathsChanged;
    }

    private void PathsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    //todo: ignore non-final items
                    foreach (var path in e.NewItems.OfType<WorldNavigationPath>())
                    {
                        _roadGraph.AddNewPath(path);
                        _pedestrianGraph.AddNewPath(path);
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
}