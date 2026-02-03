using Godot;
using JellyBrain.Scenes.Components;
using JellyBrain.Scripts.Utils;

public partial class Palourde : Node2D
{
	private Sprite2D _sprite;
	private Node2D _player;
	private Direction _playerDirection; // On which side is player located
	private SimpleMovementComponent _movementComponent;
	private Node2D _visualNode2D;// node just under the root node for animation purpose
	public AnimationPlayer _animationPlayer;
	
	
	public override void _Ready()
	{
		
		_visualNode2D = GetNode<Node2D>("VisualNode2D");
		_sprite = GetNode<Sprite2D>("VisualNode2D/Sprite2D");
		
		
		// GD.Print("OU EST LA PALOURDE");
		// GD.Print(Position.X +"   "  +  Position.Y);
		_animationPlayer = GetNode<AnimationPlayer>("VisualNode2D/AnimationPlayer");
		_animationPlayer.Play("Jump Left");
		
		_movementComponent = GetNode<SimpleMovementComponent>("SimpleMovementComponent");
		_movementComponent.ChangedDirection += _changeFacingDirection;

		
		var interestZone = GetNode<InterestZone>("InterestZone");
		interestZone.InterestedZoneEntered += _onInterestZoneEntered;
		interestZone.InterestedZoneExited += _onInterestZoneExited;
		
		if (_movementComponent == null)
		{
			GD.PrintErr("Erreur : _movementComponent est null !");
			return;
		}
		if (_sprite == null)
		{
			GD.PrintErr("Erreur : _sprite est null !");
			return;
		}

	}

	public override void _Process(double delta)
	{
		if (_playerInInterestZone())
		{
			// Face player when within interest zone
			_updatePlayerDirection();
			_changeFacingDirection(_playerDirection);
			_movementComponent.SetDirection(_playerDirection);
		}
		

	}

	private void _onInterestZoneEntered(Node2D body)
	{
		_player = body;
		_updatePlayerDirection();
		if (_animationPlayer.CurrentAnimation != "Jump Left")
		{
			_animationPlayer.Play("Jump Left");
		}
		_movementComponent.SetDirection(_playerDirection);
	}

	private void _onInterestZoneExited()
	{
		_player = null;
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
		_visualNode2D.Scale = new Vector2(newDirection == Direction.Left ? 1f : -1f, 1f);	}
}
