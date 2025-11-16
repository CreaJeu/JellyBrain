using Godot;
using JellyBrain.Scripts.Utils;

namespace JellyBrain.Scenes.Components;

public partial class SimpleMovementComponent : Node2D
{
	[Export] private float _speed;
	[Export] private Node2D _owner;
	[Export] private Vector2 _raycastOffset; // offset center point of raycast
	
	private bool _paused;
	private Direction _direction = Direction.Left;
	
	private RayCast2D _rayWallLeft;
	private RayCast2D _rayWallRight;
	private RayCast2D _rayLedgeLeft;
	private RayCast2D _rayLedgeRight;
	
	[Signal] public delegate void StartedMovingEventHandler();
	[Signal] public delegate void StoppedMovingEventHandler();
	[Signal] public delegate void ChangedDirectionEventHandler(Direction direction);
	
	public override void _Ready()
	{
		_rayWallLeft = GetNode<RayCast2D>("RayCastLeftWall");
		_rayLedgeLeft = GetNode<RayCast2D>("RayCastLeftLedge");
		_rayWallRight = GetNode<RayCast2D>("RayCastRightWall");
		_rayLedgeRight = GetNode<RayCast2D>("RayCastRightLedge");
		
		_rayWallLeft.Position += _raycastOffset;
		_rayLedgeLeft.Position += _raycastOffset;
		_rayWallRight.Position += _raycastOffset;
		_rayLedgeRight.Position += _raycastOffset;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_paused) return;
		
		CheckForObstacles();
		MoveOwner(delta);
	}
	
	private void CheckForObstacles()
	{
		if (_direction == Direction.Right)
		{
			if (_rayHitsSolid(_rayWallRight) || !_rayLedgeRight.IsColliding())
				SetDirection(Direction.Left);
		}
		else
		{
			if (_rayHitsSolid(_rayWallLeft) || !_rayLedgeLeft.IsColliding())
				SetDirection(Direction.Right);
		}
	}

	private void MoveOwner(double delta)
	{
		_owner.Position += new Vector2((int)_direction * _speed * (float)delta, 0);
	}
	
	// Don't consider player as wall
	private static bool _rayHitsSolid(RayCast2D ray)
	{
		var collider = ray.GetCollider();
		if (collider == null)
			return false;
		
		return collider is not CharacterBody2D; // TODO: replace CharacterBody2D by player class
	}

	public void SetDirection(Direction direction)
	{
		_direction = direction;
		EmitSignalChangedDirection(direction);
	}

	public Direction GetDirection()
	{
		return _direction;
	}
	
	public void PauseMovement()
	{
		_paused = true;
		EmitSignalStoppedMoving();
	}
	public void ResumeMovement()
	{
		_paused = false;
		EmitSignalStartedMoving();
	}
}
