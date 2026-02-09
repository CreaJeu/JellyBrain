using Godot;
using System;
using Godot.Collections;

public partial class StickyEnemy : CharacterBody2D
{
    [Export] private float enemySpeed = 50.0f;
    private const float GRAVITY = 200.0f;
    
    private bool gravityEnabled = true;
    private bool wasOnFloor = false, wasOnCeiling = false, wasOnWall = false;
    
    private RayCast2D brCorner, trCorner, blCorner, tlCorner;

    // Helper to determine which side of the wall we are on
    private bool isOnRightWall = false;
    private bool isOnLeftWall = false;

    private bool BottomCollision() => brCorner.IsColliding() || blCorner.IsColliding();
    private bool LeftCollision() => blCorner.IsColliding() || tlCorner.IsColliding();
    private bool TopCollision() => tlCorner.IsColliding() || trCorner.IsColliding();
    private bool RightCollision() => trCorner.IsColliding() || brCorner.IsColliding();
    
    public override void _Ready() 
    {
       brCorner = GetNode<RayCast2D>("CornerBR");
       trCorner = GetNode<RayCast2D>("CornerTR");
       blCorner = GetNode<RayCast2D>("CornerBL");
       tlCorner = GetNode<RayCast2D>("CornerTL");
    }
    
    public override void _PhysicsProcess(double delta) 
    {
       Vector2 velocity = Velocity;
       
       // Detect States
       if (IsOnFloor()) {
          wasOnFloor = true;
          wasOnCeiling = false;
          wasOnWall = false;
          isOnLeftWall = false;
          isOnRightWall = false;
       }
       
       if (IsOnWall()) {
          wasOnWall = true;
          // Determine if it's the left or right wall based on collision normal
          var normal = GetWallNormal();
          isOnLeftWall = normal.X > 0.5f;   // Wall is to our left, normal points right
          isOnRightWall = normal.X < -0.5f; // Wall is to our right, normal points left
       }
       
       if (IsOnCeiling()) {
          wasOnFloor = false;
          wasOnCeiling = true;
          wasOnWall = false;
          isOnLeftWall = false;
          isOnRightWall = false;
       }
       
       // Movement Logic
       if (gravityEnabled) {
          velocity.Y += (float)delta * GRAVITY;
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

       // Apply Visual Rotation
       ApplyRotation(delta);
    }

    private void ApplyRotation(double delta)
    {
        float targetRotation = 0f;

        if (wasOnFloor) targetRotation = 0f;
        else if (wasOnCeiling) targetRotation = Mathf.Pi; // 180 degrees
        else if (isOnLeftWall) targetRotation = Mathf.Pi / 2f; // 90 degrees
        else if (isOnRightWall) targetRotation = -Mathf.Pi / 2f; // -90 degrees

        // Smoothly rotate toward the target (lerp) for a "cooler" effect
        // If you want it instant, just use: Rotation = targetRotation;
        Rotation = (float)Mathf.LerpAngle(Rotation, targetRotation, 10f * delta);
    }
}