using System;
using System.Collections.Generic;
using LMTS.InputToolSystem.Abstract;
using LMTS.InputToolSystem.Enums;
using LMTS.InputToolSystem.Tools;

namespace LMTS.InputToolSystem;

//todo refactor to DI
public class ToolMapping: IToolMapping
{
    private readonly IDictionary<ToolType, IInputTool> _mapping;

    public ToolMapping(PlaceNavigationPathTool placeNavigationPathTool, PlaceBuildingTool placeBuildingTool, InspectTool inspectTool)
    {

        _mapping = new Dictionary<ToolType, IInputTool>()
        {
            { ToolType.PlaceNavigationPath, placeNavigationPathTool },
            { ToolType.PlaceBuilding, placeBuildingTool },
            { ToolType.Inspect, inspectTool }
        };
    }

    public IInputTool GetTool(ToolType toolType)
    {
        if (!_mapping.ContainsKey(toolType))
        {
            throw new ArgumentException($"Tool {toolType} is not defined");
        }
        
        return _mapping[toolType];
    }
}