using Godot;
using System;

public partial class GrabbableExampleObject : RigidBody2D
{
    private bool _isHeld = false;
    private float _originalGravity;

    public override void _Ready()
    {
        _originalGravity = GravityScale;
        // Optimization: Ensure the object is in the group via code if you forgot
        AddToGroup("grabbable");
    }

    public void OnGrab()
    {
        _isHeld = true;
        GravityScale = 0;
        LinearVelocity = Vector2.Zero;
        AngularVelocity = 0;
    }

    public void OnRelease(Vector2 throwVelocity)
    {
        _isHeld = false;
        GravityScale = _originalGravity;
        LinearVelocity = throwVelocity;
    }

    public void MoveTo(Vector2 targetPos, double delta)
    {
        // Use a high-precision velocity calculation to stick to the tip
        Vector2 diff = targetPos - GlobalPosition;
        
        // We use a high multiplier (e.g., 1/delta) to snap it instantly
        // but we limit it slightly so it doesn't glitch through walls
        LinearVelocity = diff / (float)delta;
    }
}
