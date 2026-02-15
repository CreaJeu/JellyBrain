using Godot;
using System;

public partial class PauseManager : Node
{
	[Export] public PauseMenu PauseMenu;
	[Export] public ColorRect DarkenBackground;
	[Export] public Player Player;

	public override void _Ready()
	{
		if (PauseMenu != null)
		{
			PauseMenu.ResumeGame += OnResumeGame;
			PauseMenu.RestartGame += RestartLevel;
		}		
		if (Player != null)
		{
			Player.PlayerDied += () =>
			{
				// CallDeferred to finish all processing before restarting scene
				CallDeferred(nameof(RestartLevel));
			};
		}
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
		bool showing = !GetTree().Paused; // Base it on the actual tree state

		PauseMenu.Visible = showing;
		if (DarkenBackground != null)
			DarkenBackground.Visible = showing;

		GetTree().Paused = showing;
    
		GetViewport().SetInputAsHandled(); 
	}
	
	private void OnResumeGame()
	{
		HidePauseMenu();
		GetTree().Paused = false;
	}

	private void HidePauseMenu()
	{
		PauseMenu.Visible = false;
		if (DarkenBackground != null)
			DarkenBackground.Visible = false;
	}

	private void RestartLevel()
	{
		OnResumeGame();
		GetTree().ReloadCurrentScene();
	}
}
