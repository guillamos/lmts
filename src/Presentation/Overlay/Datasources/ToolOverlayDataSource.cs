using System.Collections.Specialized;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.State.LocalState;

namespace LMTS.Presentation.Overlay.Datasources;

public class ToolOverlayDataSource: IOverlayDataSource
{
    private readonly OverlayDataStore _overlayDataStore;

    public ToolOverlayDataSource(OverlayDataStore overlayDataStore)
    {
        _overlayDataStore = overlayDataStore;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
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
            _overlayDataStore.ToolOverlayItems.CollectionChanged += value;
        }
        remove
        {
            _overlayDataStore.ToolOverlayItems.CollectionChanged -= value;
        }
    }
}