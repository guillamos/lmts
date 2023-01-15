using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LMTS.Presentation.Overlay.Abstract
{
    public interface IOverlayDataSource
    {
        public void Activate();

        public void Deactivate();

        public void ClickedItem(IOverlayItem item);
        public event NotifyCollectionChangedEventHandler OverlayItemCollectionChanged;
    }
}