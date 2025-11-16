using Godot;
using System;
using Godot.Collections;

public partial class StickyEnemy : CharacterBody2D
{
	[Export]
	private float enemySpeed = 50.0f;
	
	private const float GRAVITY = 200.0f;
	
	private bool gravityEnabled = true;
	private bool wasOnFloor = false,
			wasOnCeiling = false,
			wasOnWall = false;
	
	private RayCast2D brCorner,
			trCorner,
			blCorner,
			tlCorner;
	
	private bool BottomCollision() {
		return brCorner.IsColliding() || blCorner.IsColliding();
	}
	
	private bool LeftCollision() {
		return blCorner.IsColliding() || tlCorner.IsColliding();
	}
	
	private bool TopCollision() {
		return tlCorner.IsColliding() || trCorner.IsColliding();
	}
	
	private bool RightCollision() {
		return trCorner.IsColliding() || brCorner.IsColliding();
	}
	
	public override void _Ready() {
		brCorner = (RayCast2D) GetNode("CornerBR");
		trCorner = (RayCast2D) GetNode("CornerTR");
		blCorner = (RayCast2D) GetNode("CornerBL");
		tlCorner = (RayCast2D) GetNode("CornerTL");
	}
	
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		
		if (IsOnFloor()) {
			wasOnFloor = true;
			wasOnCeiling = false;
			wasOnWall = false;
		}
		
		if (IsOnWall()) {
			wasOnWall = true;
		}
		
		if (IsOnCeiling()) {
			wasOnFloor = false;
			wasOnCeiling = true;
			wasOnWall = false;
		}
		
		if (gravityEnabled) {
			velocity.Y += (float) delta * GRAVITY;
			
			if (IsOnFloor()) {
				gravityEnabled = false;
				velocity.X = enemySpeed;
				velocity.Y = enemySpeed;
			}
		}
	
		if (!BottomCollision() && wasOnFloor) {
			velocity.X = -2 * enemySpeed;
			velocity.Y = enemySpeed;
		}
		
		if (!LeftCollision() && wasOnWall && wasOnFloor) {
			velocity.X = -enemySpeed;
			velocity.Y = -2 * enemySpeed;
		}
		
		if (!TopCollision() && wasOnCeiling) {
			velocity.X = 2 * enemySpeed;
			velocity.Y = -enemySpeed;
		}
		
		if (!RightCollision() && wasOnWall && wasOnCeiling) {
			velocity.X = enemySpeed;
			velocity.Y = 2 * enemySpeed;
		}
		
		Velocity = velocity;
		
		MoveAndSlide();
	}
}
