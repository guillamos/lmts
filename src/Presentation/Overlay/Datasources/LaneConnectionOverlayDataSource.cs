using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.Navigation.NavigationGraphs;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Models;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Presentation.Overlay.Datasources;

public class LaneConnectionOverlayDataSource: BaseOverlayDataSource
{
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    private readonly RoadVehicleNavigationGraph _roadVehicleNavigationGraph;

    public LaneConnectionOverlayDataSource(IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, IWorldStateCollectionStore<WorldNavigationPath> pathCollectionStore, RoadVehicleNavigationGraph roadVehicleNavigationGraph)
    {
        _junctionCollectionStore = junctionCollectionStore;
        _pathCollectionStore = pathCollectionStore;
        _roadVehicleNavigationGraph = roadVehicleNavigationGraph;
    }

    public override void Activate()
    {
        foreach (var connection in _roadVehicleNavigationGraph.LaneConnections)
        {
            CreateOverlayItemsFromLaneConnection(connection);
        }
        _roadVehicleNavigationGraph.LaneConnections.CollectionChanged += PathsChanged;
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
                    foreach (var connection in e.NewItems.OfType<LaneConnection>())
                    {
                        CreateOverlayItemsFromLaneConnection(connection);
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

    private void CreateOverlayItemsFromLaneConnection(LaneConnection connection)
    {
        var fromPoint =
            NavigationPathGeometryUtilities.GetLaneJunctionConnectionPoint(connection.LaneFrom, connection.Junction);
        
        var toPoint =
            NavigationPathGeometryUtilities.GetLaneJunctionConnectionPoint(connection.LaneTo, connection.Junction);
            
        var color = connection.LaneFrom.Settings.Type == PathLaneType.Sidewalk ? Color.Color8(0, 255, 0) : Color.Color8(255, 0, 0);
        
        _overlayItems.Add(new OverlayLine(connection.LaneFrom.Identifier.ToString() + connection.LaneTo.Identifier.ToString(), false, fromPoint, toPoint, color, 0.4m));
    }
}