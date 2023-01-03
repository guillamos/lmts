namespace LMTS.InputToolSystem.Abstract;

public interface IInputTool
{
    public void Activate();
    public void Deactivate();

    public void ProcessTick();
}