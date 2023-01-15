using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Godot;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.Common.Models.World;
using LMTS.GUI.Abstract;
using LMTS.GUI.GUIHandlers;
using LMTS.Initialization;
using LMTS.InputHandling;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem;
using LMTS.InputToolSystem.Abstract;
using LMTS.InputToolSystem.Tools;
using LMTS.Navigation;
using LMTS.Navigation.NavigationGraphs;
using LMTS.Presentation.Overlay;
using LMTS.Presentation.Overlay.Datasources;
using LMTS.Presentation.Overlay.Enums;
using LMTS.State.LocalState;
using LMTS.State.WorldState.Abstract;
using LMTS.State.WorldState.Collections;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using Container = SimpleInjector.Container;

namespace LMTS.DependencyInjection;

public partial class DependencyInjectionSystem: Node
{
    //todo: fix this mess(GetNode does not seem to work???)
    public static DependencyInjectionSystem Instance { get; set; }
    
    private Container _container;

    public override void _EnterTree()
    {
        //todo handle this in a better way
        Instance = this;
        
        base._EnterTree();
        _container = new Container();

        // do registrations here
        _container.RegisterSingleton<IInputManager, InputManager>();
        
        _container.RegisterSingleton<IClickButtonHandler, ClickButtonHandler>();
        
        _container.RegisterSingleton<IToolManager, ToolManager>();
        _container.RegisterSingleton<IToolMapping, ToolMapping>();
        
        _container.RegisterSingleton<PlaceNavigationPathTool>();
        
        _container.RegisterSingleton<PlaceNavigationPathCommandValidator>();
        
        _container.RegisterSingleton<PathTypeInitializer>();
        
        _container.RegisterSingleton<OverlayDataStore>();
        _container.RegisterSingleton<StaticDataStore>();
        
        _container.RegisterSingleton<IWorldStateCollectionStore<WorldNavigationPath>, WorldNavigationPathCollectionStore>();
        _container.RegisterSingleton<IWorldStateCollectionStore<WorldNavigationJunction>, WorldNavigationJunctionCollectionStore>();
        
        
        _container.RegisterSingleton<LaneOverlayDataSource>();
        _container.RegisterSingleton<LaneConnectionOverlayDataSource>();
        _container.RegisterSingleton<LanesAndConnectionsOverlayDataSource>();
        
        //todo refactor this to something more extendable
        _container.RegisterInstance(new OverlayDataSourceFactory(_container)
        {
            { OverlayType.Lanes, typeof(LaneOverlayDataSource) },
            { OverlayType.LaneConnections, typeof(LaneConnectionOverlayDataSource) },
            { OverlayType.LanesAndConnections, typeof(LanesAndConnectionsOverlayDataSource) },
        });
        
        _container.RegisterSingleton<NavigationGraphManager>();
        _container.RegisterSingleton<RoadVehicleNavigationGraph>();
        
        AddMediatr(_container);
    }

    public object Resolve(Type fieldType)
    {
        return _container.GetInstance(fieldType);
    }

    //see https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.SimpleInjector/Program.cs
    private static void AddMediatr(Container container)
    {
        var assemblies = GetAssemblies().ToArray();
        container.RegisterSingleton<ServiceFactory>(() => container.GetInstance);
        container.RegisterSingleton<IMediator, Mediator>();
        container.Register(typeof(IRequestHandler<,>), assemblies);

        RegisterHandlers(container, typeof(INotificationHandler<>), assemblies);
        RegisterHandlers(container, typeof(IRequestExceptionAction<,>), assemblies);
        RegisterHandlers(container, typeof(IRequestExceptionHandler<,,>), assemblies);
        RegisterHandlers(container, typeof(IStreamRequestHandler<,>), assemblies);
        
        container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
        {
            typeof(RequestExceptionProcessorBehavior<,>),
            typeof(RequestExceptionActionProcessorBehavior<,>),
            typeof(RequestPreProcessorBehavior<,>),
            typeof(RequestPostProcessorBehavior<,>),
        });
        container.Collection.Register(typeof(IRequestPreProcessor<>), assemblies);
        container.Collection.Register(typeof(IRequestPostProcessor<,>), assemblies);
        container.Collection.Register(typeof(IStreamPipelineBehavior<,>), assemblies);

        var mediator = container.GetInstance<IMediator>();
    }
    
    private static void RegisterHandlers(Container container, Type collectionType, Assembly[] assemblies)
    {
        // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
        var handlerTypes = container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
        {
            IncludeGenericTypeDefinitions = true,
            IncludeComposites = false,
        });

        container.Collection.Register(collectionType, handlerTypes);
    }

    private static IEnumerable<Assembly> GetAssemblies()
    {
        yield return typeof(IMediator).GetTypeInfo().Assembly;
        yield return typeof(DependencyInjectionSystem).Assembly;
    }
}