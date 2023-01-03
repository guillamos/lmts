using LMTS.InputToolSystem.Enums;

namespace LMTS.InputToolSystem.Abstract;

public interface IToolMapping
{
    public IInputTool GetTool(ToolType toolType);
}