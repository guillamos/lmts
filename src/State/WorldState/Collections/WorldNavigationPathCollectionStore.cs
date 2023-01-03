using LMTS.Common.Models;
using LMTS.Common.Models.World;
using LMTS.State.WorldState.Abstract;

namespace LMTS.State.WorldState.Collections;

/// <summary>
/// Stores all edges used for navigation e.g. roads, railroads, paths etc which are persisted and may be rendered in the world, see <see cref="WorldNavigationPath"/>
/// </summary>
public class WorldNavigationPathCollectionStore: WorldStateCollectionStore<WorldNavigationPath>
{

}