using System.Threading;
using System.Threading.Tasks;
using LMTS.CommandSystem.Commands.LocalCommands;
using LMTS.InputToolSystem.Abstract;
using MediatR;

namespace LMTS.CommandSystem.Handlers.LocalCommandHandlers;

public class ActivateToolCommandHandler: IRequestHandler<ActivateToolCommand>
{
    private readonly IToolManager _toolManager;
    private readonly IToolMapping _toolMapping;

    public ActivateToolCommandHandler(IToolManager toolManager, IToolMapping toolMapping)
    {
        _toolManager = toolManager;
        _toolMapping = toolMapping;
    }

    public Task<Unit> Handle(ActivateToolCommand request, CancellationToken cancellationToken)
    {
        var tool = _toolMapping.GetTool(request.Type);
        _toolManager.SetActiveTool(tool, request.ExtraData);
        
        return Task.FromResult(Unit.Value);
    }
}