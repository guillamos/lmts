using System;
using LMTS.Common.Enums;

namespace LMTS.Common.Abstract;

public interface IWorldObject
{
    public WorldObjectState State { get; set; }
    public Guid Identifier { get; }
}