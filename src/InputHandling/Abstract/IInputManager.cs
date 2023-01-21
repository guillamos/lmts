using System.Collections.Generic;
using LMTS.Common.Models.Input;

namespace LMTS.InputHandling.Abstract;

public interface IInputManager
{
    void AddMousePickObjectsForTick(IEnumerable<RayPickedObject> input);
    void ClearClickInputForTick();
    IEnumerable<RayPickedObject> GetHoveredObjectsForTick();
    IEnumerable<RayPickedObject> GetClickedObjectsForTick();
    void SetClicked();
}