using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LMTS.Common.Constants;
using LMTS.Common.Enums;
using LMTS.Common.Models.World;
using LMTS.Common.Utilities;
using LMTS.CommonServices.Abstract;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Models;
using LMTS.State.LocalState;
using LMTS.State.WorldState.Abstract;

namespace LMTS.InputToolSystem.Tools;

//this is mostly for random debugging now
public class InspectTool: IInputTool
{
    private readonly IInputManager _inputManager;
    private readonly OverlayDataStore _overlayDataStore;
    private readonly IWorldStateCollectionStore<WorldBuilding> _buildingCollectionStore;
    private readonly IPathInteractionPointService _pathInteractionPointService;

    public List<IOverlayItem> currentOverlayItems = new();

    public InspectTool(IInputManager inputManager, OverlayDataStore overlayDataStore, IWorldStateCollectionStore<WorldBuilding> buildingCollectionStore, IPathInteractionPointService pathInteractionPointService)
    {
        _inputManager = inputManager;
        _overlayDataStore = overlayDataStore;
        _buildingCollectionStore = buildingCollectionStore;
        _pathInteractionPointService = pathInteractionPointService;
    }

    public void Activate(string extraData)
    {
    }

    public void Deactivate()
    {
        ClearCurrentOverlayItems();
    }

    public void ProcessTick()
    {
        var clickedItems = _inputManager.GetClickedObjectsForTick();

        foreach (var clickedItem in clickedItems)
        {
            if (clickedItem.Node.HasMeta(MetadataConstants.MetaTypeKey))
            {
                var metaType = clickedItem.Node.GetMeta(MetadataConstants.MetaTypeKey);
                if (metaType.ToString() == MetadataConstants.MetaTypeBuilding)
                {
                    var buildingId = new Guid(clickedItem.Node.GetMeta(MetadataConstants.MetaTypeIdKey).ToString());
                    var matchedBuilding =
                        _buildingCollectionStore.Items.FirstOrDefault(item => item.Identifier == buildingId);
                    
                    var pathConnectingPoint =
                        _pathInteractionPointService.GetClosestInteractionPoint(matchedBuilding.OriginPosition, 2);
                    
                    //todo should be centrally defined
                    var validPathLaneTypes = new List<PathLaneType>() { PathLaneType.BasicRoad };
                    
                    //todo make getting closest lane a utility function somewhere
                    var closestPathLane = pathConnectingPoint.Side == PathInteractionPointSide.Left
                        ? pathConnectingPoint.Path.Lanes.FirstOrDefault(pl =>
                            validPathLaneTypes.Contains(pl.Value.Settings.Type))
                        : pathConnectingPoint.Path.Lanes.LastOrDefault(pl =>
                            validPathLaneTypes.Contains(pl.Value.Settings.Type));
                    
                    //todo make rendering a lane a utility function somewhere
                    var closestlaneMiddle = NavigationPathGeometryUtilities.GetLaneMiddleOffset(closestPathLane.Value);
                    

                    var closestLaneFrom = NavigationPathGeometryUtilities.GetRelativePositionAlongPath(closestPathLane.Value.Path, 0, closestlaneMiddle);
                    var closestLaneTo = NavigationPathGeometryUtilities.GetRelativePositionAlongPath(closestPathLane.Value.Path, 1, closestlaneMiddle);

                    var color = closestPathLane.Value.Settings.Type == PathLaneType.Sidewalk ? Color.Color8(0, 255, 0) : Color.Color8(255, 0, 0);

                    var closestLaneOverlayItem = new OverlayLine(closestPathLane.Value.Identifier.ToString(), false, closestLaneFrom, closestLaneTo,
                        color, 0.4m);
            
                    ClearCurrentOverlayItems();
                    _overlayDataStore.ToolOverlayItems.Add(closestLaneOverlayItem);
                    currentOverlayItems.Add(closestLaneOverlayItem);
                    
                    //Geometry2D.LineIntersectsLine()
                    
                    var spawnPoint = pathConnectingPoint.Position - pathConnectingPoint.Normal * (float)(closestPathLane.Value.Settings.Offset * 2 + closestPathLane.Value.Settings.Width / 2);
                    
                    color = Color.Color8(0, 255, 0);

                    var spawnOverlayItem = new OverlayLine(closestPathLane.Value.Identifier.ToString(), false, pathConnectingPoint.Position, spawnPoint,
                        color, 0.4m);
                    
                    _overlayDataStore.ToolOverlayItems.Add(spawnOverlayItem);
                    currentOverlayItems.Add(spawnOverlayItem);
                }
            }
        }
    }

    private void ClearCurrentOverlayItems()
    {
        foreach (var overlayItem in currentOverlayItems)
        {
            _overlayDataStore.ToolOverlayItems.Remove(overlayItem);
        }
        currentOverlayItems.Clear();
    }
}