using Godot;
using System;

public partial class PauseMenu : Control
{
	private Button _exitButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_exitButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/ExitButton");
		_exitButton.Connect("pressed", new Callable(this, "_on_ExitButton_pressed"));
	}

	private void _on_ExitButton_pressed()
	{
		GetTree().Quit();
	}

}
