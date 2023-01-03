using System.Collections.Generic;
using LMTS.InputToolSystem.Abstract;
using LMTS.InputToolSystem.Enums;
using LMTS.InputToolSystem.Tools;

namespace LMTS.InputToolSystem;

public class ToolMapping: IToolMapping
{
    private readonly IDictionary<ToolType, IInputTool> _mapping;

    public ToolMapping(PlaceNavigationPathTool placeNavigationPathTool)
    {

        _mapping = new Dictionary<ToolType, IInputTool>()
        {
            { ToolType.PlaceNavigationPath, placeNavigationPathTool }
        };
    }

    public IInputTool GetTool(ToolType toolType)
    {
        if (!_mapping.ContainsKey(toolType))
        {
            return null;
        }
        
        return _mapping[toolType];
    }
}