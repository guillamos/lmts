using LMTS.CommandSystem.Commands.WorldCommands;

namespace LMTS.CommandSystem.Validators.WorldCommandValidators;

public class PlaceNavigationPathCommandValidator
{
    //todo implement validation or something. and think about how to show the feedback in the gui?
    public bool IsValid(PlaceNavigationPathCommand command)
    {
        if (command.FromJunction == command.ToJunction)
        {
            return false;
        }
        
        return true;
    }
}