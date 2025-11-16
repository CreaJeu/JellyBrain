using Godot;
using System;

public partial class Player: CharacterBody2D
{
	private const float GRAVITY = 200.0f;
	
	[Export]
	public float acceleration { get; set; } = 1000.0f;
	
	[Export]
	public float maxSpeed { get; set; } = 150.0f;
	
	[Export]
	public float friction { get; set; } = 0.975f;
	
	[Export]
	public float climbVelocity { get; set; } = 40.0f;
	
	private HealthComponent healthComponent;
	private Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		healthComponent.HealthBelowZero += _onBelowZeroHealth;
		healthComponent.HealthChanged += _onHealthChanged;
	}
	
	public override void _Process(double delta) {
		var velocity = Velocity;
		
		if (Input.IsActionPressed("move_right")) {
			if (velocity.X < maxSpeed) {
				velocity.X += acceleration * (float) delta;
			}
			
			if (IsOnWall()) {
				velocity.Y = -climbVelocity;
			}
		}
		
		if (Input.IsActionPressed("move_left")) {
			if (velocity.X > -maxSpeed) {
				velocity.X -= acceleration * (float) delta;
			}
			
			if (IsOnWall()) {
				velocity.Y = -climbVelocity;
			}
		}
		
		if (!Input.IsActionPressed("move_left") && !Input.IsActionPressed("move_right")) {
			velocity.X *= friction;
		}
		
		Velocity = velocity;
	}

	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;
		velocity.Y += (float)delta * GRAVITY;
		Velocity = velocity;

		var motion = Velocity * (float)delta;
		MoveAndSlide();
	}

	private void _onBelowZeroHealth(int newHealth)
	{
		GD.Print("Yer DEAD!");
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_sprite, "modulate", Colors.Red, 1.0f);

	}
	
	private void _onHealthChanged(int oldHealth, int newHealth)
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_sprite, "modulate", Colors.White, 1.0f);
		GD.Print($"oldHealth: {oldHealth}, newHealth: {newHealth}");
		
	}

}
