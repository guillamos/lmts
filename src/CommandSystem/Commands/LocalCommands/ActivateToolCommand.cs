using LMTS.InputToolSystem.Enums;
using MediatR;

namespace LMTS.CommandSystem.Commands.LocalCommands;

public class ActivateToolCommand: IRequest
{
    public ActivateToolCommand(ToolType type, string extraData)
    {
        Type = type;
        ExtraData = extraData;
    }

    public ToolType Type { get; set; }
    
    public string ExtraData { get; set; }
}