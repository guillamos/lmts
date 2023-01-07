using System;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.DependencyInjection;
using LMTS.Presentation.Utilities;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Presentation;

public partial class NavigationJunctionPresenter: Node3D
{
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    /*[Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;*/

    private Material _asphaltMaterial;
    private Material _sidewalkMaterial;

    public override void _Ready()
    {
        this.ResolveDependencies();

        _asphaltMaterial = ResourceLoader.Load<Material>("res://assets/material/asphalt.tres");
        _sidewalkMaterial = ResourceLoader.Load<Material>("res://assets/material/sidewalk.tres");
        
        //todo listen for property changes
        _junctionCollectionStore.Items.CollectionChanged += JunctionsChanged;
        
        //todo listen for path changes as these influence junctions
    }

    private void JunctionsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var junction in e.NewItems.OfType<WorldNavigationJunction>())
                    {
                        RenderJunction(junction);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Replace:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Move:
                throw new NotImplementedException();
                break;
            case NotifyCollectionChangedAction.Reset:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RenderJunction(WorldNavigationJunction newJunction)
    {
        //translation for preventing z fighting
        var baseTranslation = new Vector3(0, 0.3f, 0);
        var translatedJunctionPosition = newJunction.Position + baseTranslation;
        
        //todo: think of how to actually render a junction

        var newArrayMesh = new ArrayMesh();
        
        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);

        var currentIndex = 0;
        var offsetVector1 = new Vector3(-5, 0, -5) + translatedJunctionPosition;
        var offsetVector2 = new Vector3(-5, 0, 5) + translatedJunctionPosition;
        var offsetVector3 = new Vector3(5, 0, 5) + translatedJunctionPosition;
        var offsetVector4 = new Vector3(5, 0, -5) + translatedJunctionPosition;
        
        currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, offsetVector1, offsetVector2, offsetVector3, offsetVector4, true);
        
        st.GenerateNormals();
        st.GenerateTangents();
        st.Index();
        
        newArrayMesh = st.Commit(newArrayMesh);
        
        var newMeshInstance = new MeshInstance3D();
        
        newMeshInstance.Mesh = newArrayMesh;

        newMeshInstance.CreateTrimeshCollision();

        var newCollision = newMeshInstance.GetChildren().OfType<StaticBody3D>().FirstOrDefault();
        
        NodeUtilities.SetWorldObjectMeta(newCollision, newJunction);
        NodeUtilities.SetWorldObjectMeta(newMeshInstance, newJunction);

        AddChild(newMeshInstance);
        
    }
}