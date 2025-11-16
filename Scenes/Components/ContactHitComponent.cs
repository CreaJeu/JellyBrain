using Godot;

// Damages hittable objects on contact
public partial class ContactHitComponent : Area2D
{
	[Export] private int _damage = 1;
	[Export] private CollisionShape2D _hitbox; // A copy of this collider will be assigned to the component
	[Export] private double _cooldownDuration = 1; // Only hits when not on cooldown
	[Export] private bool _singleUse = true; // Disappears after hit
	[Export] private bool _hitEnemies = false;
	[Export] private bool _hitPlayerAndAllies = true;
	[Export] private bool _startOnCooldown = false;

	private CollisionShape2D _ownHitbox; // Actual hitbox
	private Node2D _affectedBody;
	private bool _hitting = false;
	private bool _onCooldown = false;
	private Timer _cooldownTimer;

	[Signal]
	public delegate void HitHittableObjectEventHandler(int damage);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ownHitbox = GetNode<CollisionShape2D>("HitBox");
		_ownHitbox.Name = $"{Name}_Interactable_Collider";
		_ownHitbox.Shape = (Shape2D)_hitbox.Shape.Duplicate();
		_ownHitbox.Position = _hitbox.Position;
		_ownHitbox.Rotation = _hitbox.Rotation;
		_ownHitbox.Scale = _hitbox.Scale;

		_cooldownTimer = new Timer();
		_cooldownTimer.Timeout += _onCooldownTimeout;
		AddChild(_cooldownTimer);
		if (_startOnCooldown) _startCooldown();
		
		BodyEntered += _onBodyEntered;
		BodyExited += _onBodyExited;
	}

	private void _startCooldown()
	{
		GD.Print("Starting cooldown");
		_onCooldown = true;
		_cooldownTimer.Start(_cooldownDuration);
	}

	private void _onCooldownTimeout()
	{
		GD.Print("Cooldown ended");
		_onCooldown = false;
	}

	public override void _Process(double delta)
	{
		if (!_hitting || _onCooldown) return;
		
		_hit();
	}

	private void _onBodyEntered(Node2D body)
	{
		if (_hitEnemies && body.IsInGroup("enemies") || _hitPlayerAndAllies && (body.IsInGroup("player") || body.IsInGroup("allies")))
		{
			GD.Print(body);
			_affectedBody = body;
		}
		else return;
		
		_hitting = true;
		
		if (body.HasNode("HealthComponent"))
		{
			var health = body.GetNode<HealthComponent>("HealthComponent");
			Connect(SignalName.HitHittableObject, new Callable(health, nameof(HealthComponent.ApplyDamage)));
		}

		if (!_onCooldown) _hit();
	}

	private void _onBodyExited(Node2D body)
	{
		if (body != _affectedBody) return;
		
		_hitting = false;
		_affectedBody = null;
	}

	private void _hit()
	{
		EmitSignalHitHittableObject(_damage);
		_startCooldown();
	}

}
