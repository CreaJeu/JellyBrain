using Godot;
using System;

public partial class PauseMenu : Control
{
	[Signal]
	public delegate void ResumeGameEventHandler();
	
	private Button _exitButton;
	private Button _resumeButton;
	private Button _settingsButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_exitButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/ExitButton");
		_exitButton.Pressed += _onExitButtonPressed;
		
		_resumeButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/ResumeButton");
		_resumeButton.Pressed += _onResumeButtonPressed;
		
		_settingsButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/SettingsButton");
		_settingsButton.Pressed += _onSettingsButtonPressed;
	}

	private void _onExitButtonPressed()
	{
		GetTree().Quit();
	}

	private void _onResumeButtonPressed()
	{
		EmitSignal(SignalName.ResumeGame);
	}

	private void _onSettingsButtonPressed()
	{
		GD.Print("Settings opened");
	}
}
