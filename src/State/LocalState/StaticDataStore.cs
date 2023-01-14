using System.Collections.Generic;
using LMTS.Common.Models.StaticData;

namespace LMTS.State.LocalState;

public class StaticDataStore
{
    public void Initialize(IEnumerable<PathType> pathTypes)
    {
        PathTypes = pathTypes;
    }

    public IEnumerable<PathType> PathTypes { get; private set; }
}