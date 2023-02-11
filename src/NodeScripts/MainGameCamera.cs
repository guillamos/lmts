using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using LMTS.Common.Models.Input;
using LMTS.DependencyInjection;
using LMTS.InputHandling.Abstract;

namespace LMTS.NodeScripts;

public partial class MainGameCamera : Camera3D
{
	[Export(PropertyHint.Range, "0, 10, 0.01")]
	private float _sensitivity = 3;
	
	[Export(PropertyHint.Range, "0, 1000, 0.1")]
	private float _velocity = 5;

	[Inject] private IInputManager _inputManager;

	private bool isRotating = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ResolveDependencies();
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	
	public override void _UnhandledInput(InputEvent inputEvent) {
		if (isRotating)
		{
			if(inputEvent is InputEventMouseMotion mouseEvent)
			{
				var newVector = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
				newVector.Y -= mouseEvent.Relative.X / 1000 * _sensitivity;
				newVector.X -= mouseEvent.Relative.Y / 1000 * _sensitivity;
				newVector.X = (float)Math.Clamp(Rotation.X, Math.PI / -2, Math.PI / 2);

				Rotation = newVector;
			}
		}

		if (inputEvent is InputEventMouseButton mouseButtonEvent)
		{
			switch (mouseButtonEvent.ButtonIndex)
			{
				case MouseButton.Left:
					if (inputEvent.IsPressed())
					{
						_inputManager.SetClicked();
					}
					break;
				case MouseButton.Right:
					if (inputEvent.IsPressed())
					{
						isRotating = true;
					}
					else
					{
						isRotating = false;
					}
					break;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var direction = new Vector3(
			(Input.IsKeyPressed(Key.D) ? 1 : 0) - (Input.IsKeyPressed(Key.A) ? 1 : 0),
			(Input.IsKeyPressed(Key.E) ? 1 : 0) - (Input.IsKeyPressed(Key.Q) ? 1 : 0),
			(Input.IsKeyPressed(Key.S) ? 1 : 0) - (Input.IsKeyPressed(Key.W) ? 1 : 0)
		).Normalized();

		Translate(direction * _velocity * (float)delta);
		
		PickMouseCameraRay();
	}

	private void PickMouseCameraRay()
	{
		var mousePos = GetViewport().GetMousePosition();
		var rayFrom = ProjectRayOrigin(mousePos);
		const int rayLength = 1000;
		var rayTo = rayFrom + ProjectRayNormal(mousePos) * rayLength;
		var spaceState = GetWorld3D().DirectSpaceState;
		
		var rayQuery = PhysicsRayQueryParameters3D.Create(rayFrom, rayTo);
		var selection = spaceState.IntersectRay(rayQuery);

		var hitObjects = new List<RayPickedObject>();

		while(selection.Any())
		{
			var colliderObject = selection["collider"].AsGodotObject();
			var hitPosition = (Vector3)selection["position"];

			if (colliderObject is CollisionObject3D node)
			{
				hitObjects.Add(new RayPickedObject(node, hitPosition));
				rayQuery.Exclude = new Godot.Collections.Array<Rid>(hitObjects.Select(ho => ho.Node.GetRid()));
				selection = spaceState.IntersectRay(rayQuery);
			}
			else
			{
				break;
			}
		}
		
		_inputManager.AddMousePickObjectsForTick(hitObjects);
	}
}