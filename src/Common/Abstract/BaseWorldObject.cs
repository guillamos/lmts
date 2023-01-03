using System;
using LMTS.Common.Enums;

namespace LMTS.Common.Abstract;

public class BaseWorldObject: IWorldObject
{
    public BaseWorldObject(WorldObjectState state)
    {
        State = state;
        Identifier = Guid.NewGuid();
    }

    public WorldObjectState State { get; set; }
    public Guid Identifier { get; }
}