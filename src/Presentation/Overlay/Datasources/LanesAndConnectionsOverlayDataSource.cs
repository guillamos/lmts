using System.Collections.Specialized;
using LMTS.Presentation.Overlay.Abstract;

namespace LMTS.Presentation.Overlay.Datasources;

//todo not sure if this is the right way to combine datasources
public class LanesAndConnectionsOverlayDataSource : IOverlayDataSource
{
    private readonly LaneConnectionOverlayDataSource _laneConnectionOverlayDataSource;
    private readonly LaneOverlayDataSource _laneOverlayDataSource;

    public LanesAndConnectionsOverlayDataSource(LaneConnectionOverlayDataSource laneConnectionOverlayDataSource, LaneOverlayDataSource laneOverlayDataSource)
    {
        _laneConnectionOverlayDataSource = laneConnectionOverlayDataSource;
        _laneOverlayDataSource = laneOverlayDataSource;
    }

    public void Activate()
    {
        _laneOverlayDataSource.Activate();
        _laneConnectionOverlayDataSource.Activate();
    }

    public void Deactivate()
    {
        _laneOverlayDataSource.Deactivate();
        _laneConnectionOverlayDataSource.Deactivate();
    }

    //todo no idea how to handle this
    public void ClickedItem(IOverlayItem item)
    {
        throw new System.NotImplementedException();
    }
    
    public event NotifyCollectionChangedEventHandler OverlayItemCollectionChanged
    {
        add
        {
            _laneOverlayDataSource.OverlayItemCollectionChanged += value;
            _laneConnectionOverlayDataSource.OverlayItemCollectionChanged += value;
        }
        remove
        {
            _laneOverlayDataSource.OverlayItemCollectionChanged -= value;
            _laneConnectionOverlayDataSource.OverlayItemCollectionChanged -= value;
        }
    }
}