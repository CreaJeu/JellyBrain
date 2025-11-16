using Godot;
using System;

public partial class Interactable : Area2D
{
	[Export] private CollisionShape2D _collisionShape; // A copy of this collider will be assigned to the interactable
	[Export] private Vector2 _labelOffset = Vector2.Zero;
	[Signal] public delegate void InteractedWithEventHandler();
	
	private CollisionShape2D _ownCollider;
	private RichTextLabel _label;
	private bool _playerInRange;

	public override void _Ready()
	{
		if (_collisionShape == null)
		{
			GD.PrintErr($"{Name} Collision shape not set");
			return;
		}
		// Copy collision
		_ownCollider = GetNode<CollisionShape2D>("CollisionShape2D");
		_ownCollider.Name = $"{Name}_Interactable_Collider";
		_ownCollider.Shape = (Shape2D)_collisionShape.Shape.Duplicate();
		_ownCollider.Rotation = _collisionShape.Rotation;
		_ownCollider.Scale = _collisionShape.Scale;
		
		_label = GetNode<RichTextLabel>("InteractLabel");
		_label.Position += _labelOffset;

		BodyEntered += _onBodyEntered;
		BodyExited += _onBodyExited;
	}

	private void _onBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("player")) return;
		_label.Visible = true;
		_playerInRange = true;
	}

	private void _onBodyExited(Node2D body)
	{
		if (!body.IsInGroup("player")) return;
		_label.Visible = false;
		_playerInRange = false;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_playerInRange) return;
		base._UnhandledInput(@event);
		if (@event.IsActionPressed("interact"))
		{
			_onInteractKeyPressed();
		}
	}


	private void _onInteractKeyPressed()
	{
		EmitSignal(SignalName.InteractedWith);
	}
}
