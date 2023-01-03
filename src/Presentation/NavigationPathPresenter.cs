using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.Common.Models.World;
using LMTS.DependencyInjection;
using LMTS.State.WorldState.Abstract;
using LMTS.State.WorldState.Collections;

namespace LMTS.Presentation;

public partial class NavigationPathPresenter: Node3D
{
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    
    [Inject]
    private readonly IWorldStateCollectionStore<WorldNavigationPath> _pathCollectionStore;

    public override void _Ready()
    {
        this.ResolveDependencies();
        
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
        //todo render with lanes and stuff
        var fromPosition = newPath.From.Position + new Vector3(0, .1f, 0);
        var toPosition = newPath.To.Position + new Vector3(0, .1f, 0);
        
        var directionVector = fromPosition.DirectionTo(toPosition).Rotated(Vector3.Up, (float)Math.PI / 2);
        var directionVectorInverse = Vector3.Zero - directionVector;
        
        var point1 = fromPosition + directionVector;
        var point2 = fromPosition + directionVectorInverse;
        
        var point3 = toPosition + directionVectorInverse;
        var point4 = toPosition + directionVector;
        
        var newMeshInstance = new MeshInstance3D();
        
        var st = new SurfaceTool();
        
        st.Begin(Mesh.PrimitiveType.Triangles);
        
        var currentIndex = 0;
        
        currentIndex = RenderQuad(st, currentIndex, point4, point3, point2, point1);
        
        st.GenerateNormals();
        st.Index();

        var newMesh = st.Commit();
        newMeshInstance.Mesh = newMesh;

        AddChild(newMeshInstance);
    }

    //todo split to some drawing utilities class
    //todo determine corners automatically
    static int RenderQuad(SurfaceTool st, int currentIndex, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        st.SetUv(new Vector2(0, 1));
        st.AddVertex(v1);

        st.SetUv(new Vector2(0, 0));
        st.AddVertex(v2);

        st.SetUv(new Vector2(1, 0));
        st.AddVertex(v3);
        
        st.SetUv(new Vector2(1, 1));
        st.AddVertex(v4);
        
        st.AddIndex(currentIndex + 0);
        st.AddIndex(currentIndex + 1);
        st.AddIndex(currentIndex + 2);

        st.AddIndex(currentIndex + 0);
        st.AddIndex(currentIndex + 2);
        st.AddIndex(currentIndex + 3);
        return currentIndex + 4;
    }
    
}