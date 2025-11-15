using Godot;
using System;

public partial class Player: CharacterBody2D
{
	private const float GRAVITY = 200.0f;
	
	[Export]
	public float acceleration { get; set; } = 1000.0f;
	
	[Export]
	public float maxSpeed { get; set; } = 500.0f;
	
	[Export]
	public float friction { get; set; } = 0.975f;
	
	[Export]
	public float climbVelocity { get; set; } = 100f;
	
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
}
