using System;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.DependencyInjection;
using LMTS.Presentation.Utilities;
using LMTS.State.WorldState.Abstract;

namespace LMTS.Presentation.World;

public partial class BuildingPresenter: Node3D
{
    [Inject]
    private readonly IWorldStateCollectionStore<WorldBuilding> _buildingCollectionStore;
    
    private Material _buildingMaterial;

    public override void _Ready()
    {
        this.ResolveDependencies();

        _buildingMaterial = ResourceLoader.Load<Material>("res://assets/material/overlay.tres");

        //todo listen for property changes
        _buildingCollectionStore.Items.CollectionChanged += BuildingsChanged;
        
        //todo listen for path changes as these influence junctions
    }

    private void BuildingsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var building in e.NewItems.OfType<WorldBuilding>())
                    {
                        RenderBuilding(building);
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

    private void RenderBuilding(WorldBuilding newBuilding)
    {
        //translation for preventing z fighting
        var baseTranslation = new Vector3(0, 0.3f, 0);
        var translatedBuildingPosition = newBuilding.OriginPosition + baseTranslation;
        
        //todo: think of how to actually render a building

        var newArrayMesh = new ArrayMesh();
        
        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);
        
        st.SetMaterial(_buildingMaterial);

        var currentIndex = 0;
        var offsetVector1 = new Vector3(-2, 0, -2) + translatedBuildingPosition;
        var offsetVector2 = new Vector3(-2, 0, 2) + translatedBuildingPosition;
        var offsetVector3 = new Vector3(2, 0, 2) + translatedBuildingPosition;
        var offsetVector4 = new Vector3(2, 0, -2) + translatedBuildingPosition;

        currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, offsetVector1, offsetVector2, offsetVector3, offsetVector4, true, new Color(255, 0, 0));
        
        st.GenerateNormals();
        st.GenerateTangents();
        st.Index();
        
        newArrayMesh = st.Commit(newArrayMesh);
        
        var newMeshInstance = new MeshInstance3D();
        
        newMeshInstance.Mesh = newArrayMesh;

        newMeshInstance.CreateTrimeshCollision();

        var newCollision = newMeshInstance.GetChildren().OfType<StaticBody3D>().First();
        
        NodeUtilities.SetWorldObjectMeta(newCollision, newBuilding);
        NodeUtilities.SetWorldObjectMeta(newMeshInstance, newBuilding);

        AddChild(newMeshInstance);
        
    }
}