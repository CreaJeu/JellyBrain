using Godot;
using System;
using JellyBrain.Scripts.Utils;

public partial class StaticNpc : Node2D
{
	private AnimatedSprite2D _sprite;
	private Node2D _player;
	private Direction _playerDirection; // On which side is player located
	private SimpleMovementComponent _movementComponent;
	
	public override void _Ready()
	{
		var interactable = GetNode<Interactable>("Interactable");
		interactable.InteractedWith += _onInteract;
		
		_movementComponent = GetNode<SimpleMovementComponent>("SimpleMovementComponent");
		_movementComponent.ChangedDirection += _changeDirection;
		_movementComponent.StoppedMoving += _onMovementPaused;
		_movementComponent.StartedMoving += _onMovementResumed;

		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sprite.Play();
		
		var interestZone = GetNode<InterestZone>("InterestZone");
		interestZone.InterestedZoneEntered += _onInterestZoneEntered;
		interestZone.InterestedZoneExited += _onInterestZoneExited;
	}

	public override void _Process(double delta)
	{
		if (!_playerInInterestZone()) return;
		
		// Face player when within interest zone
		_playerDirection = _player.Position.X > GlobalPosition.X ? Direction.Right : Direction.Left;
		_changeDirection(_playerDirection);
	}
	
	private void _onInteract()
	{
		GD.Print($"{Name} Received interact and can do talk!");
	}

	// ------ Interest zone
	private void _onInterestZoneEntered(Node2D body)
	{
		_player = body;
		_sprite.Animation = "angry";
		_sprite.Play();
		_playerDirection = _player.Position.X > GlobalPosition.X ? Direction.Right : Direction.Left;
		_changeDirection(_playerDirection);
		_movementComponent.PauseMovement();
	}

	private void _onInterestZoneExited()
	{
		_player = null;
		_sprite.Animation = "default";
		_sprite.Play();
		_movementComponent.ResumeMovement();
	}

	private bool _playerInInterestZone()
	{
		return _player != null;
	}

	// ------ Movement
	private void _changeDirection(Direction newDirection)
	{
		_sprite.FlipH = newDirection == Direction.Right;
	}

	private void _onMovementPaused()
	{
	}

	private void _onMovementResumed()
	{
		_changeDirection(_movementComponent.GetDirection());
	}

}
