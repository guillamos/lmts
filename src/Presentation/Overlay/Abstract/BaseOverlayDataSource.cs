using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LMTS.Presentation.Overlay.Abstract;

public abstract class BaseOverlayDataSource: IOverlayDataSource
{
    protected ObservableCollection<IOverlayItem> _overlayItems = new ObservableCollection<IOverlayItem>();
    
    public abstract void Activate();

    public abstract void Deactivate();

    public abstract void ClickedItem(IOverlayItem item);

    public event NotifyCollectionChangedEventHandler OverlayItemCollectionChanged
    {
        add => _overlayItems.CollectionChanged += value;
        remove => _overlayItems.CollectionChanged -= value;
    }
}