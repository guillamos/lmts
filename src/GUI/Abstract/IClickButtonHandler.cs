using LMTS.GUI.Enums;

namespace LMTS.GUI.Abstract;

public interface IClickButtonHandler
{
    void HandleButtonAction(ButtonAction action, string actionData);
}