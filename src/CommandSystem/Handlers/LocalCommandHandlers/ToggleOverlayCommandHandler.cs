using System.Threading;
using System.Threading.Tasks;
using LMTS.CommandSystem.Commands.LocalCommands;
using LMTS.State.LocalState;
using MediatR;

namespace LMTS.CommandSystem.Handlers.LocalCommandHandlers;

public class ToggleOverlayCommandHandler: IRequestHandler<ToggleOverlayCommand>
{
    private readonly OverlayDataStore _overlayDataStore;

    public ToggleOverlayCommandHandler(OverlayDataStore overlayDataStore)
    {
        _overlayDataStore = overlayDataStore;
    }

    public Task<Unit> Handle(ToggleOverlayCommand request, CancellationToken cancellationToken)
    {
        if (_overlayDataStore.ActiveOverlay.Value == request.Type)
        {
            _overlayDataStore.ActiveOverlay.OnNext(null);
        }
        else
        {
            _overlayDataStore.ActiveOverlay.OnNext(request.Type);
        }

        
        return Task.FromResult(Unit.Value);
    }
}