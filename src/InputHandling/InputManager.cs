using System.Collections.Generic;
using System.Linq;
using LMTS.Common.Models.Input;
using LMTS.InputHandling.Abstract;

namespace LMTS.InputHandling;

public class InputManager: IInputManager
{
    private IEnumerable<RayPickedObject> LastPickedObjects { get; set; }
    private bool Clicked { get; set; }
    
    public void AddMousePickObjectsForTick(IEnumerable<RayPickedObject> input)
    {
        LastPickedObjects = input;
    }

    public void ClearClickInputForTick()
    {
        Clicked = false;
        LastPickedObjects = null;
    }

    public IEnumerable<RayPickedObject> GetHoveredObjectsForTick()
    {
        return LastPickedObjects;
    }
    
    public IEnumerable<RayPickedObject> GetClickedObjectsForTick()
    {
        return Clicked ? LastPickedObjects : Enumerable.Empty<RayPickedObject>();
    }
    
    public void SetClicked()
    {
        Clicked = true;
    }
}