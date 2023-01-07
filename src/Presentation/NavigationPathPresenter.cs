using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Constants;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.DependencyInjection;
using LMTS.Presentation.Utilities;
using LMTS.State.WorldState.Abstract;
using LMTS.State.WorldState.Collections;

namespace LMTS.Presentation;

public partial class NavigationPathPresenter: Node3D
{
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    private Material _asphaltMaterial;
    private Material _sidewalkMaterial;

    public override void _Ready()
    {
        this.ResolveDependencies();

        _asphaltMaterial = ResourceLoader.Load<Material>("res://assets/material/asphalt.tres");
        _sidewalkMaterial = ResourceLoader.Load<Material>("res://assets/material/sidewalk.tres");
        
        //todo listen for property changes
        _pathCollectionStore.Items.CollectionChanged += PathsChanged;
    }

    private void PathsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var path in e.NewItems.OfType<WorldNavigationPath>())
                    {
                        RenderPath(path);
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

    private void RenderPath(WorldNavigationPath newPath)
    {
        //translation for preventing z fighting
        var baseTranslation = new Vector3(0, 0.1f, 0);
        
        var fromPosition = newPath.From.Position + baseTranslation;
        var toPosition = newPath.To.Position + baseTranslation;
        
        var perpendicularDirectionVector = fromPosition.DirectionTo(toPosition).Rotated(Vector3.Up, (float)Math.PI / 2);

        var newArrayMesh = new ArrayMesh();
        
        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);
        
        foreach (var lane in newPath.PathType.Lanes)
        {
            st = new SurfaceTool();

            st.Begin(Mesh.PrimitiveType.Triangles);
            
            var currentIndex = 0;
            
            var fromPoint = fromPosition + (perpendicularDirectionVector * (float)lane.Offset);
            var fromPointOffset = fromPosition + (perpendicularDirectionVector * ((float)lane.Offset + (float)lane.Width));
            
            var toPoint = toPosition + (perpendicularDirectionVector * (float)lane.Offset);
            var toPointOffset = toPosition + (perpendicularDirectionVector * ((float)lane.Offset + (float)lane.Width));
            
            switch (lane.Type)
            {
                case PathLaneType.Sidewalk:
                    st.SetMaterial(_sidewalkMaterial);
                    //todo: separate method?
                    var translation = new Vector3(0, 0.1f, 0);
                    //render main surface
                    currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, fromPoint + translation, fromPointOffset + translation, toPointOffset + translation, toPoint + translation);
                    
                    //render left kerb
                    currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, fromPoint + translation, fromPoint, toPoint, toPoint + translation, true);
                    
                    //render right kerb
                    currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, fromPointOffset + translation, fromPointOffset, toPointOffset, toPointOffset + translation);
                    
                    break;
                case PathLaneType.BasicRoad:
                    st.SetMaterial(_asphaltMaterial);
                    currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, fromPoint, fromPointOffset, toPointOffset, toPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            st.GenerateNormals();
            st.GenerateTangents();
            st.Index();

            newArrayMesh = st.Commit(newArrayMesh);
        }
        
        var newMeshInstance = new MeshInstance3D();
        
        newMeshInstance.Mesh = newArrayMesh;

        newMeshInstance.CreateTrimeshCollision();

        var newCollision = newMeshInstance.GetChildren().OfType<StaticBody3D>().FirstOrDefault();
        
        NodeUtilities.SetWorldObjectMeta(newCollision, newPath);
        NodeUtilities.SetWorldObjectMeta(newMeshInstance, newPath);

        AddChild(newMeshInstance);
        
    }
    
}