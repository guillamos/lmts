using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using Godot;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.DependencyInjection;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Models;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Presentation.Overlay.Datasources;

public class LaneOverlayDataSource: IOverlayDataSource
{
    private ObservableCollection<IOverlayItem> _overlayItems = new ObservableCollection<IOverlayItem>();
    
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    public LaneOverlayDataSource(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
    }

    public void Activate()
    {
        foreach (var path in _pathCollectionStore.Items)
        {
            CreateOverlayItemsFromWorldPath(path);
        }
        _pathCollectionStore.Items.CollectionChanged += PathsChanged;
    }

    public void Deactivate()
    {
        _pathCollectionStore.Items.CollectionChanged -= PathsChanged;
        _overlayItems.Clear();
    }

    public ObservableCollection<IOverlayItem> GetOverlayItems()
    {
        return _overlayItems;
    }

    public void ClickedItem(IOverlayItem item)
    {
        throw new System.NotImplementedException();
    }
    
    private void PathsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var path in e.NewItems.OfType<WorldNavigationPath>())
                    {
                        CreateOverlayItemsFromWorldPath(path);
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

    private void CreateOverlayItemsFromWorldPath(WorldNavigationPath path)
    {
        foreach (var lane in path.Lanes)
        {
            var laneMiddle = lane.Value.Offset + (lane.Value.Width / 2);

            var from = NavigationPathGeometryUtilties.GetRelativePositionAlongPath(path, 0, laneMiddle);
            var to = NavigationPathGeometryUtilties.GetRelativePositionAlongPath(path, 1, laneMiddle);
            _overlayItems.Add(new OverlayLine(path.Identifier + "-" + lane.Key, false, from, to, Color.Color8(255, 0, 0), 0.4m));
        }
    }
}