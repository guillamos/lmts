using System.Reactive.Subjects;
using LMTS.Presentation.Overlay.Enums;

namespace LMTS.State.LocalState;

public class OverlayDataStore
{
    public BehaviorSubject<OverlayType?> ActiveOverlays { get; set; } = new(null);
}