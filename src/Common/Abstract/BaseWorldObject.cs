using System;
using LMTS.Common.Enums;

namespace LMTS.Common.Abstract;

public class BaseWorldObject: IWorldObject
{
    public BaseWorldObject(Guid identifier, WorldObjectState state)
    {
        Identifier = identifier;
        State = state;
    }

    public WorldObjectState State { get; set; }
    public Guid Identifier { get; }
}