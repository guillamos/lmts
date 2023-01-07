namespace LMTS.InputToolSystem.Abstract;

public interface IToolManager
{
    public void SetActiveTool(IInputTool tool, string extraData);

    public void UnsetActiveTool();

    public void ProcessActiveTool();
}