using Godot;
using JellyBrain.Scenes.Components;
using JellyBrain.Scripts.Utils;

public partial class Palourde : Node2D
{
	private AnimatedSprite2D _sprite;
	private Node2D _player;
	private Direction _playerDirection; // On which side is player located
	private SimpleMovementComponent _movementComponent;
	
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sprite.Animation = "movement";
		_sprite.Play();
		
		_movementComponent = GetNode<SimpleMovementComponent>("SimpleMovementComponent");
		_movementComponent.ChangedDirection += _changeFacingDirection;

		
		var interestZone = GetNode<InterestZone>("InterestZone");
		interestZone.InterestedZoneEntered += _onInterestZoneEntered;
		interestZone.InterestedZoneExited += _onInterestZoneExited;
	}

	public override void _Process(double delta)
	{
		if (!_playerInInterestZone()) return;
		
		// Face player when within interest zone
		_updatePlayerDirection();
		_changeFacingDirection(_playerDirection);
		_movementComponent.SetDirection(_playerDirection);
		GD.Print("  OUI LA PALOURDE EXISTE AU SECOUR");
	}

	private void _onInterestZoneEntered(Node2D body)
	{
		_player = body;
		_sprite.Animation = "palourdeClosing";
		_sprite.Play();
		_updatePlayerDirection();
		_movementComponent.SetDirection(_playerDirection);
	}

	private void _onInterestZoneExited()
	{
		_player = null;
		_sprite.Animation = "movement";
		_sprite.Play();
	}
	
	private bool _playerInInterestZone()
	{
		return _player != null;
	}

	private void _updatePlayerDirection()
	{
		_playerDirection = _player.Position.X > GlobalPosition.X ? Direction.Right : Direction.Left;
	}
	
	private void _changeFacingDirection(Direction newDirection)
	{
		_sprite.FlipH = newDirection == Direction.Right;
	}
}
