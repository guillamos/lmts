using LMTS.Common.Models;
using LMTS.Common.Models.World;
using LMTS.State.WorldState.Abstract;

namespace LMTS.State.WorldState.Collections;

/// <summary>
/// Stores all junction nodes used for navigation which are persisted and may be rendered in the world, see <see cref="WorldNavigationJunction"/>
/// </summary>
public class WorldNavigationJunctionCollectionStore: WorldStateCollectionStore<WorldNavigationJunction>
{

}