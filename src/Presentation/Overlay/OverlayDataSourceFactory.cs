using System.Collections.Generic;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Enums;
using SimpleInjector;

namespace LMTS.Presentation.Overlay;

public class OverlayDataSourceFactory: Dictionary<OverlayType, System.Type>
{
    private readonly Container container;

    public OverlayDataSourceFactory(Container container)
    {
        this.container = container;
    }

    public IOverlayDataSource Create(OverlayType overlayType) =>
        (IOverlayDataSource)this.container.GetInstance(this[overlayType]);
}