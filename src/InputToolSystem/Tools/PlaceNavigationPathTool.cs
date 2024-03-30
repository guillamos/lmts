﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using LMTS.CommandSystem.Commands.WorldCommands;
using LMTS.CommandSystem.Validators.WorldCommandValidators;
using LMTS.Common.Constants;
using LMTS.Common.Enums;
using LMTS.Common.Models.StaticData;
using LMTS.Common.Models.World;
using LMTS.DependencyInjection;
using LMTS.InputHandling;
using LMTS.InputHandling.Abstract;
using LMTS.InputToolSystem.Abstract;
using LMTS.Presentation.Overlay.Abstract;
using LMTS.Presentation.Overlay.Models;
using LMTS.State.LocalState;
using LMTS.State.WorldState.Abstract;
using LMTS.State.WorldState.Collections;
using MediatR;

namespace LMTS.InputToolSystem.Tools;

public class PlaceNavigationPathTool: IInputTool
{
    private readonly IInputManager _inputManager;
    private readonly StaticDataStore _staticDataStore;
    private readonly PlaceNavigationPathCommandValidator _placeNavigationPathCommandValidator;
    private readonly IWorldStateCollectionStore<WorldNavigationJunction> _junctionCollectionStore;
    private readonly IMediator _mediator;
    private readonly OverlayDataStore _overlayDataStore;

    private WorldNavigationJunction? _firstClickedJunction;
    private PathType? _pathType;

    public List<IOverlayItem> currentOverlayItems = new();

    public PlaceNavigationPathTool(IInputManager inputManager, PlaceNavigationPathCommandValidator placeNavigationPathCommandValidator, IMediator mediator, StaticDataStore staticDataStore, IWorldStateCollectionStore<WorldNavigationJunction> junctionCollectionStore, OverlayDataStore overlayDataStore)
    {
        _inputManager = inputManager;
        _placeNavigationPathCommandValidator = placeNavigationPathCommandValidator;
        _mediator = mediator;
        _staticDataStore = staticDataStore;
        _junctionCollectionStore = junctionCollectionStore;
        _overlayDataStore = overlayDataStore;
    }

    public void Activate(string extraData)
    {
        var matchingPathType = _staticDataStore.PathTypes.FirstOrDefault(pathType => pathType.Key == extraData);
        _pathType = matchingPathType ?? _staticDataStore.PathTypes.FirstOrDefault();
    }

    public void Deactivate()
    {
        
    }

    public void ProcessTick()
    {
        var hoveredItems = _inputManager.GetHoveredObjectsForTick();

        ClearCurrentOverlayItems();
        if (hoveredItems != null)
        {
            foreach (var hoveredItem in hoveredItems)
            {
                if (hoveredItem.Node.HasMeta(MetadataConstants.MetaTypeKey))
                {
                    var metaType = hoveredItem.Node.GetMeta(MetadataConstants.MetaTypeKey);
                    if (metaType.ToString() == MetadataConstants.MetaTypeNavigationPath)
                    {
                        continue;
                    }
                    if (metaType.ToString() == MetadataConstants.MetaTypeNavigationJunction)
                    {

                    }
                }

                //todo maybe get floor by script or metadata or something
                else if (hoveredItem.Node.Name == "FloorPlane")
                {
                    //create a new junction on this position
                    //todo: check if this is a valid position for a junction
                    //clickedJunction = new WorldNavigationJunction(WorldObjectState.PreviewGhost, hoveredItem.PickedPosition);

                    if (_firstClickedJunction != null)
                    {
                        var spawnOverlayItem = new OverlayLine("GhostNavigationPath", false, _firstClickedJunction.Position, hoveredItem.PickedPosition, Color.Color8(0, 255, 0), 0.4m);

                        _overlayDataStore.ToolOverlayItems.Add(spawnOverlayItem);
                        currentOverlayItems.Add(spawnOverlayItem);
                    }

                    break;
                }
            }
        }


        var clickedItems = _inputManager.GetClickedObjectsForTick();

        if (clickedItems == null)
        {
            return;
        }

        WorldNavigationJunction clickedJunction = null;

        foreach (var clickedItem in clickedItems)
        {
            if (clickedItem.Node.HasMeta(MetadataConstants.MetaTypeKey))
            {
                var metaType = clickedItem.Node.GetMeta(MetadataConstants.MetaTypeKey);
                if (metaType.ToString() == MetadataConstants.MetaTypeNavigationPath)
                {
                    continue;
                }
                if (metaType.ToString() == MetadataConstants.MetaTypeNavigationJunction)
                {
                    var junctionId = new Guid(clickedItem.Node.GetMeta(MetadataConstants.MetaTypeIdKey).ToString());
                    var matchedJunction =
                        _junctionCollectionStore.Items.FirstOrDefault(item => item.Identifier == junctionId);

                    if (matchedJunction != null)
                    {
                        clickedJunction = matchedJunction;
                        break;
                    }
                }
            }
            
            //todo check if clicked existing junction
            
            //todo maybe get floor by script or metadata or something
            else if (clickedItem.Node.Name == "FloorPlane")
            {
                //create a new junction on this position
                //todo: check if this is a valid position for a junction
                clickedJunction = new WorldNavigationJunction(WorldObjectState.PreviewGhost, clickedItem.PickedPosition);
                break;
            }
        }

        if (clickedJunction != null)
        {
            if (_firstClickedJunction == null)
            {
                _firstClickedJunction = clickedJunction;
            }
            else
            {
                var placeCommand = new PlaceNavigationPathCommand(_firstClickedJunction, clickedJunction, _pathType);

                var validationResult = _placeNavigationPathCommandValidator.IsValid(placeCommand);
                if (validationResult)
                {
                    _mediator.Send(placeCommand);
                    _firstClickedJunction = null;
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