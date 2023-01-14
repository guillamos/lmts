using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Godot;
using LMTS.DependencyInjection;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Enums;
using LMTS.Presentation.Overlay.Models;
using LMTS.Presentation.Utilities;
using LMTS.State.LocalState;

namespace LMTS.Presentation.Overlay;

public partial class OverlayPresenter: Node3D
{
    [Inject]
    private readonly OverlayDataSourceFactory _overlayDataSourceFactory;

    [Inject] 
    private readonly OverlayDataStore _overlayDataStore;

    private IOverlayDataSource _dataSource;

    private readonly IDictionary<IOverlayItem, Node3D> _renderedItems = new Dictionary<IOverlayItem, Node3D>();
    
    private Material _overlayMaterial;

    public override void _EnterTree()
    {
        this.ResolveDependencies();
        
        _overlayDataStore.ActiveOverlay.Subscribe(SetDataSource);
        
        _overlayMaterial = ResourceLoader.Load<Material>("res://assets/material/overlay.tres");
    }

    private void SetDataSource(OverlayType? type)
    {
        if (_dataSource != null)
        {
            foreach (var item in _dataSource.GetOverlayItems())
            {
                RemoveRenderedOverlayItem(item);
            }
            _dataSource.GetOverlayItems().CollectionChanged -= OverlayItemsChanged;
            _dataSource.Deactivate();
        }

        if (type != null)
        {
            _dataSource = _overlayDataSourceFactory.Create(type.Value);
        }
        else
        {
            _dataSource = null;
        }

        if (_dataSource != null)
        {
            _dataSource.Activate();
            foreach (var item in _dataSource.GetOverlayItems())
            {
                RenderOverlayItem(item);
            }
            _dataSource.GetOverlayItems().CollectionChanged += OverlayItemsChanged;
        }
    }
    
    private void OverlayItemsChanged (object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.OfType<IOverlayItem>())
                    {
                        RenderOverlayItem(item);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.OfType<IOverlayItem>())
                    {
                        RemoveRenderedOverlayItem(item);
                    }
                }
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

    public void RenderOverlayItem(IOverlayItem item)
    {
        //translation for preventing z fighting
        var baseTranslation = new Vector3(0, 0.4f, 0);

        var newArrayMesh = new ArrayMesh();
        
        var st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Triangles);
        
        var currentIndex = 0;

        if (item is OverlayLine line)
        {
            st.SetMaterial(_overlayMaterial);
            var perpendicularDirectionVector = line.From.DirectionTo(line.To).Rotated(Vector3.Up, (float)Math.PI / 2);
            var widthHalf = (float)(line.Width / 2);

            var fromPoint = line.From - (perpendicularDirectionVector * widthHalf) + baseTranslation;
            var fromPointOffset = line.From + (perpendicularDirectionVector * widthHalf) + baseTranslation;
            var toPoint = line.To - (perpendicularDirectionVector * widthHalf) + baseTranslation;
            var toPointOffset = line.To + (perpendicularDirectionVector * widthHalf) + baseTranslation;
            currentIndex = DrawingUtilities.RenderQuad(st, currentIndex, fromPoint, fromPointOffset, toPointOffset, toPoint, false, line.Color);
        }

        st.GenerateNormals();
        st.GenerateTangents();
        st.Index();

        newArrayMesh = st.Commit(newArrayMesh);

        var newMeshInstance = new MeshInstance3D();
        
        newMeshInstance.Mesh = newArrayMesh;
        newMeshInstance.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

        //todo create click/hover hit box

        AddChild(newMeshInstance);
        
        _renderedItems.Add(item, newMeshInstance);
    }
    
    public void RemoveRenderedOverlayItem(IOverlayItem item)
    {
        if(_renderedItems.TryGetValue(item, out var sceneItem))
        {
            RemoveChild(sceneItem);
            _renderedItems.Remove(item);
        }
    }
}