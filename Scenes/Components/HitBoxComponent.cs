
using Godot;

public partial class HitBoxComponent : Area2D
{
	[Export] private int _maxHealthPoints = 10;
	[Export] private CollisionShape2D _hitbox; // A copy of this collider will be assigned to the component
	[Export] private float _cooldown; // Can't be hit for the duration of the cooldown

	private int _currentHealthPoints;
	private CollisionShape2D _ownHitbox; // Actual hitbox

	[Signal]
	public delegate void DamageTakenEventHandler(int oldHealthPoints, int newHealthPoints);
	[Signal]
	public delegate void HealthBelowZeroEventHandler();

	public override void _Ready()
	{
		_currentHealthPoints = _maxHealthPoints;
		
		_ownHitbox = GetNode<CollisionShape2D>("HitBox");
		_ownHitbox.Name = $"{Name}_Interactable_Collider";
		_ownHitbox.Shape = (Shape2D)_hitbox.Shape.Duplicate();
		_ownHitbox.Rotation = _hitbox.Rotation;
		_ownHitbox.Scale = _hitbox.Scale;
	}
	
	// private void _onBodyEntered(Node2D body)
	// {
	// 	if (!body.IsInGroup("player")) return;
	// 	_label.Visible = true;
	// 	_playerInRange = true;
	// }
	//
	// private void _onBodyExited(Node2D body)
	// {
	// 	if (!body.IsInGroup("player")) return;
	// 	_label.Visible = false;
	// 	_playerInRange = false;
	// }
}
