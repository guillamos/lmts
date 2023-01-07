namespace LMTS.InputToolSystem.Abstract;

public interface IInputTool
{
    public void Activate(string extraData);
    public void Deactivate();

    public void ProcessTick();
}