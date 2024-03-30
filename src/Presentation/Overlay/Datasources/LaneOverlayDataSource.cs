using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Models;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Presentation.Overlay.Datasources;

public class LaneOverlayDataSource: BaseOverlayDataSource
{
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    public LaneOverlayDataSource(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
    }

    public override void Activate()
    {
        foreach (var path in _pathCollectionStore.Items.Values)
        {
            CreateOverlayItemsFromWorldPath(path);
        }
        _pathCollectionStore.Items.CollectionChanged += PathsChanged;
    }

    public override void Deactivate()
    {
        _pathCollectionStore.Items.CollectionChanged -= PathsChanged;
        _overlayItems.Clear();
    }

    public ObservableCollection<IOverlayItem> GetOverlayItems()
    {
        return _overlayItems;
    }

    public override void ClickedItem(IOverlayItem item)
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
            var laneMiddle = NavigationPathGeometryUtilities.GetLaneMiddleOffset(lane.Value);

            var from = NavigationPathGeometryUtilities.GetRelativePositionAlongPath(path, 0, laneMiddle);
            var to = NavigationPathGeometryUtilities.GetRelativePositionAlongPath(path, 1, laneMiddle);

            var color = lane.Value.Settings.Type == PathLaneType.Sidewalk ? Color.Color8(0, 255, 0) : Color.Color8(255, 0, 0);
            
            _overlayItems.Add(new OverlayLine(lane.Value.Identifier.ToString(), false, from, to, color, 0.4m));
        }
    }
}