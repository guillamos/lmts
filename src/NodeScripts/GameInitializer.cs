using Godot;
using LMTS.DependencyInjection;
using LMTS.Initialization;

namespace LMTS.NodeScripts;

public partial class GameInitializer: Node
{
    [Inject] private PathTypeInitializer _pathTypeInitializer;
    
    public override void _EnterTree()
    {
        this.ResolveDependencies();
        _pathTypeInitializer.Initialize();
    }
}