using Godot;
using System;
using System.Collections.Generic;

public partial class Tentacules : Node2D
{
    [Export] public int TentaculeLength = 10;
    [Export] public float DistanceBetweenParts = 5.0f;
    [Export] public float LineWidth = 2.0f;

    [Export] public float jointsSoftness = 0.1f;
    [Export] public float jointsBias = 1f;
    [Export] public Node2D PlayerNode2D;
    
    private List<RigidBody2D> _segments = new List<RigidBody2D>();
    private Line2D _line;
    
    private PinJoint2D _grabJoint;
    private RigidBody2D _grabbedObject;

    public override void _Ready()
    {
        // 1. Line Visual Setup
        _line = new Line2D();
        _line.Width = LineWidth;
        _line.Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png");
        _line.TextureMode = Line2D.LineTextureMode.Tile; 
        _line.TextureFilter = TextureFilterEnum.Nearest;
        
        // Ensure the line is drawn behind the player or at top level
        AddChild(_line);

        Vector2 spawnPos = PlayerNode2D.GlobalPosition + new Vector2(0, 5);
        Node2D parentToAttachTo = PlayerNode2D;

        for (int i = 0; i < TentaculeLength; i++)
        {
            RigidBody2D segment = new RigidBody2D();
        
            // 1. Physical positioning
            segment.GlobalPosition = spawnPos + new Vector2(0, (i + 1) * DistanceBetweenParts);
        
            // 2. STABILITY: Freeze the segment so it doesn't move during setup
            segment.Freeze = true; 
        
            segment.Mass = 0.1f;
            segment.CollisionLayer = 4;
            segment.CollisionMask = 1; 
            segment.LinearDamp = 3.0f; // High damping stops the "crazy" movement
            segment.AngularDamp = 3.0f;

            CollisionShape2D shape = new CollisionShape2D();
            shape.Shape = new CircleShape2D { Radius = 3.0f }; // Smaller radius = less overlap
            segment.AddChild(shape);

            AddChild(segment);
            _segments.Add(segment);

            PinJoint2D joint = new PinJoint2D();
            joint.GlobalPosition = parentToAttachTo.GlobalPosition;
            joint.NodeA = parentToAttachTo.GetPath();
            joint.NodeB = segment.GetPath();
        
            joint.Softness = jointsSoftness;
            joint.Bias = jointsBias;
            joint.DisableCollision = true; // MUST BE TRUE

            AddChild(joint);
            parentToAttachTo = segment;
        }

        // 3. WAIT: Tell the segments to wake up only AFTER everything is connected
        CallDeferred(MethodName.UnfreezeSegments);
    }

    private void UnfreezeSegments()
    {
        foreach (var segment in _segments)
        {
            segment.Freeze = false;
        }
    }

    public override void _Process(double delta)
    {
        _line.ClearPoints();
        
        _line.AddPoint(_line.ToLocal(PlayerNode2D.GlobalPosition));

        foreach (var segment in _segments)
        {
            _line.AddPoint(_line.ToLocal(segment.GlobalPosition));
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_segments.Count > 0)
        {
            var lastSegment = _segments[^1];
            var mousePos = GetGlobalMousePosition();
            var direction = mousePos - lastSegment.GlobalPosition;
        
            // Stronger pull if farther away
            lastSegment.ApplyCentralForce(direction * 500.0f);
            
            // Damping helps with the "laggy" elastic feel
            lastSegment.LinearVelocity *= 0.95f;
            lastSegment.AngularVelocity *= 0.95f;
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("grab")) 
        {
            TryGrab();
        }
        else if (@event.IsActionReleased("grab"))
        {
            ReleaseGrab();
        }
    }

    private void TryGrab()
    {
        var tip = _segments[^1];
        var spaceState = GetWorld2D().DirectSpaceState;
    
        var query = new PhysicsPointQueryParameters2D();
        query.Position = tip.GlobalPosition;
        query.CollisionMask = 1; 
    
        var result = spaceState.IntersectPoint(query);

        if (result.Count > 0)
        {
            GD.Print("Grabbed Object!");
            var hit = result[0];
            Variant colliderVariant = hit["collider"];
            
            if (colliderVariant.Obj is RigidBody2D target)
            {
                _grabbedObject = target;
            
                _grabJoint = new PinJoint2D();
                // Attach the joint to the world at the tip's current position
                _grabJoint.GlobalPosition = tip.GlobalPosition;
            
                _grabJoint.NodeA = tip.GetPath();
                _grabJoint.NodeB = _grabbedObject.GetPath();
                
                // Allow the object to move smoothly
                _grabJoint.Softness = 0.1f;
            
                AddChild(_grabJoint);
            }
        }
    }

    private void ReleaseGrab()
    {
        if (_grabJoint != null)
        {
            GD.Print("Released Object");
            _grabJoint.QueueFree();
            _grabJoint = null;
            _grabbedObject = null;
        }
    }
}