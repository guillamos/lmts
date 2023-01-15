using System.Collections.Generic;
using LMTS.Common.Enums;

namespace LMTS.Navigation.NavigationGraphs;

public class RoadVehicleNavigationGraph: PathNavigationGraph
{
    private static IEnumerable<PathLaneType> validLaneTypes = new[] { PathLaneType.BasicRoad };
    
    public RoadVehicleNavigationGraph() : base(validLaneTypes)
    {

    }
}