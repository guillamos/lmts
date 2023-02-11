using System.Collections.ObjectModel;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Enums;

namespace LMTS.State.LocalState;

public class OverlayDataStore
{
    public ObservableCollection<OverlayType> ActiveOverlays { get; set; } = new();

    public ObservableCollection<IOverlayItem> ToolOverlayItems = new();
}