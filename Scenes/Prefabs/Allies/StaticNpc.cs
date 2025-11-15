using Godot;
using System;

public partial class StaticNpc : Node2D
{
	private AnimatedSprite2D _sprite;
	private Node2D _player;
	
	public override void _Ready()
	{
		var interactable = GetNode<Interactable>("Interactable");
		interactable.InteractedWith += _onInteract;

		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sprite.Play();
		
		var interestZone = GetNode<InterestZone>("InterestZone");
		interestZone.InterestedZoneEntered += _onInterestZoneEntered;
		interestZone.InterestedZoneExited += _onInterestZoneExited;
	}

	public override void _Process(double delta)
	{
		// flip animation to face player ?
		if (_player is null) return;
		_sprite.FlipH = _player.Position.X > GlobalPosition.X;
	}

	private void _onInteract()
	{
		GD.Print($"{Name} Received interact and can do talk!");
	}

	private void _onInterestZoneEntered(Node2D body)
	{
		_player = body;
		_sprite.Animation = "angry";
		_sprite.Play();
	}

	private void _onInterestZoneExited()
	{
		_player = null;
		_sprite.Animation = "default";
		_sprite.Play();
	}

}
