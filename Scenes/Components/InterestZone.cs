using Godot;
using System;

// Sends signals when player enters the zone
public partial class InterestZone : Area2D
{
	[Export] private int _radius;
	[Signal] public delegate void InterestedZoneEnteredEventHandler(Node2D body);
	[Signal] public delegate void InterestedZoneExitedEventHandler();

	private CollisionShape2D _collisionShape;
	
	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_collisionShape.Shape = new CircleShape2D{Radius = _radius};
		
		BodyEntered += _onBodyEntered;
		BodyExited += _onBodyExited;
	}

	private void _onBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("player")) return;
		EmitSignalInterestedZoneEntered(body);
	}

	private void _onBodyExited(Node2D body)
	{
		if (!body.IsInGroup("player")) return;
		EmitSignalInterestedZoneExited();
	}
}
