using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LMTS.Common.Models.World;
using LMTS.Navigation.Abstract;
using LMTS.Navigation.NavigationGraphs;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Navigation;

//todo is this class necessary?
public class NavigationGraphManager
{
    private IList<INavigationGraph> _navigationGraphs = new List<INavigationGraph>();
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    public NavigationGraphManager(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore, RoadVehicleNavigationGraph roadVehicleNavigationGraph)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
        _pathCollectionStore.Items.CollectionChanged += PathsChanged;
        _navigationGraphs.Add(roadVehicleNavigationGraph);
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
                        foreach (var graph in _navigationGraphs)
                        {
                            graph.AddNewPath(path);
                        }
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