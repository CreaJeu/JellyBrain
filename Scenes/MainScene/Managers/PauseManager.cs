using Godot;
using System;

public partial class PauseManager : Node
{
	[Export] public PauseMenu PauseMenu;
	[Export] public ColorRect DarkenBackground;

	public override void _Ready()
	{
		if (PauseMenu != null)
			PauseMenu.ResumeGame += OnResumeGame;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
		if (@event.IsActionPressed("pause_game"))
		{
			TogglePauseMenu();
		}
	}
	
	private void TogglePauseMenu()
	{
		bool showing = !PauseMenu.Visible;

		PauseMenu.Visible = showing;
		if (DarkenBackground != null)
			DarkenBackground.Visible = showing;

		GetTree().Paused = showing;
	}
	
	private void OnResumeGame()
	{
		HidePauseMenu();
	}

	private void HidePauseMenu()
	{
		PauseMenu.Visible = false;
		if (DarkenBackground != null)
			DarkenBackground.Visible = false;

		GetTree().Paused = false;
	}
}
