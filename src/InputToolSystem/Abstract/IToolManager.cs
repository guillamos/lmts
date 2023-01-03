namespace LMTS.InputToolSystem.Abstract;

public interface IToolManager
{
    public void SetActiveTool(IInputTool tool);

    public void UnsetActiveTool();

    public void ProcessActiveTool();
}