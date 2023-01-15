using System;
using System.Collections.ObjectModel;
using LMTS.Common.Models.Navigation;
using LMTS.Common.Models.World;

namespace LMTS.Navigation.Abstract;

public interface INavigationGraph
{
    void AddNewPath(WorldNavigationPath path);
    ObservableCollection<LaneConnection> LaneConnections { get; }
}