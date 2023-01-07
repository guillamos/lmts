using LMTS.InputToolSystem.Abstract;

namespace LMTS.InputToolSystem;

public class ToolManager: IToolManager
{
    private IInputTool ActiveTool { get; set; }
    
    public void SetActiveTool(IInputTool tool, string extraData)
    {
        if (ActiveTool != null)
        {
            ActiveTool.Deactivate();
        }

        ActiveTool = tool;
        ActiveTool.Activate(extraData);
    }

    public void UnsetActiveTool()
    {
        if (ActiveTool != null)
        {
            ActiveTool.Deactivate();
        }
    }

    public void ProcessActiveTool()
    {
        if (ActiveTool != null)
        {
            ActiveTool.ProcessTick();
        }
    }
}