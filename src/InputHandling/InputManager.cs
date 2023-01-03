using System.Collections.Generic;
using LMTS.Common.Models.Input;
using LMTS.InputHandling.Abstract;

namespace LMTS.InputHandling;

public class InputManager: IInputManager
{
    private IEnumerable<RayPickedObject> _lastClickPickedObjects { get; set; }
    
    public void AddClickInputForTick(IEnumerable<RayPickedObject> input)
    {
        _lastClickPickedObjects = input;
    }

    public void ClearClickInputForTick()
    {
        _lastClickPickedObjects = null;
    }

    public IEnumerable<RayPickedObject> GetPickedObjectsForTick()
    {
        return _lastClickPickedObjects;
    }
}