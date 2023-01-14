using LMTS.Presentation.Overlay.Enums;
using MediatR;

namespace LMTS.CommandSystem.Commands.LocalCommands;

//todo: are overlays able to be activated or are they always linked with a tool?
public class ToggleOverlayCommand : IRequest
{
    public ToggleOverlayCommand(OverlayType type)
    {
        Type = type;
    }

    public OverlayType Type { get; set; }
}