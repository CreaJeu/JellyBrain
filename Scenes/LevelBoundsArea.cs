using Godot;
using System;

public partial class LevelBoundsArea : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			_ = player.OnOutOfBounds();
		}
	}
}
