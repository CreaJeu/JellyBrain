using Godot;
using System;

public partial class PauseMenu : Control
{
	[Signal]
	public delegate void ResumeGameEventHandler();
	[Signal]
	public delegate void RestartGameEventHandler();
	
	public override void _Ready()
	{
		var exitButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/ExitButton");
		exitButton.Pressed += _onExitButtonPressed;
		
		var resumeButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/ResumeButton");
		resumeButton.Pressed += _onResumeButtonPressed;
		
		var settingsButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/SettingsButton");
		settingsButton.Pressed += _onSettingsButtonPressed;
		
		var restartButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/RestartButton");
		restartButton.Pressed += _onRestartButtonPressed;

	}

	private void _onExitButtonPressed()
	{
		GetTree().Quit();
	}
	
	private void _onRestartButtonPressed()
	{
		EmitSignal(SignalName.RestartGame);
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
