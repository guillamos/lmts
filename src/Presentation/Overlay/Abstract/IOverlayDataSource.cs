using System.Collections.ObjectModel;

namespace LMTS.Presentation.Overlay.Abstract
{
    public interface IOverlayDataSource
    {
        public void Activate();

        public void Deactivate();
        public ObservableCollection<IOverlayItem> GetOverlayItems();

        public void ClickedItem(IOverlayItem item);
    }
}