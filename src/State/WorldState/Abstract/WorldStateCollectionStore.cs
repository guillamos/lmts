using System.Collections.ObjectModel;
using LMTS.Common.Abstract;

namespace LMTS.State.WorldState.Abstract;

public abstract class WorldStateCollectionStore<T>: IWorldStateCollectionStore<T> where T: IWorldObject
{
    //todo make this private and expose only an indexed copy?
    public ObservableCollection<T> Items { get; } = new();
}