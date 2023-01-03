using LMTS.InputToolSystem.Enums;
using MediatR;

namespace LMTS.CommandSystem.Commands.LocalCommands;

public class ActivateToolCommand: IRequest
{
    public ActivateToolCommand(ToolType type)
    {
        Type = type;
    }

    public ToolType Type { get; set; }
}