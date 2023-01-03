using System.Collections.ObjectModel;
using LMTS.Common.Abstract;

namespace LMTS.State.WorldState.Abstract;

/// <summary>
/// A collection store of items in the world which are persisted over saves and over multiplayer synchronization
/// </summary>
/// <typeparam name="T">The type of item to store in this collection</typeparam>
public interface IWorldStateCollectionStore<T> where T : IWorldObject
{
    public ObservableCollection<T> Items { get; }
}