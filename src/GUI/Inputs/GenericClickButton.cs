using System.Diagnostics;
using Godot;
using LMTS.DependencyInjection;
using LMTS.GUI.Abstract;
using LMTS.GUI.Enums;

namespace LMTS.GUI.Inputs;

public partial class GenericClickButton : Button
{
	[Export]
	private ButtonAction _action;
	
	[Export]
	private string _actionData;

	[Inject] private IClickButtonHandler _clickButtonHandler;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ResolveDependencies();
		this.ButtonDown += OnButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed()
	{
		_clickButtonHandler.HandleButtonAction(_action, _actionData);
	}
}