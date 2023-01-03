using System.Collections.Generic;
using LMTS.Common.Models.Input;

namespace LMTS.InputHandling.Abstract;

public interface IInputManager
{
    void AddClickInputForTick(IEnumerable<RayPickedObject> input);
    IEnumerable<RayPickedObject> GetPickedObjectsForTick();
    void ClearClickInputForTick();
}