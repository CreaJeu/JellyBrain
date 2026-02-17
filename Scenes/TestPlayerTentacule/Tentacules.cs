using Godot;
using System;
using System.Collections.Generic;

public partial class Tentacules : Node2D
{
    [Export] public int TentaculeLength = 10;
    [Export] public float DistanceBetweenParts = 6.0f;
    [Export] public float LineWidth = 2.0f; 
    [Export] public float PushStrength =  1.0f;

    [Export] public Node2D PlayerNode2D;
    
    
    
    private List<RigidBody2D> _segments = new List<RigidBody2D>();
    private Line2D _line;
    
    private int _currentSegmentCount = 0;
    private int _targetSegmentCount = 10;
    private Node2D _lastAttachedNode;

    public override void _Ready()
    {
        // _line = CreateLine(150);
        _line = CreateLine();
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("grab"))
        {
            foreach (var s in _segments) s.QueueFree();
            _segments.Clear();
            
             GrowTentacle(10,GetGlobalMousePosition());
        }
    }

    /**
     * This should push the last segment of the tentacule toward the target
     */
    private void PushToward(Vector2 positionToPush)
    {
        if (_segments.Count == 0) return;

        RigidBody2D lastSegment = _segments[_segments.Count - 1];

        
        
        GD.Print(positionToPush);
        GD.Print(PlayerNode2D.Position);

        Vector2 direction = positionToPush - lastSegment.GlobalPosition;
        lastSegment.ApplyCentralImpulse(direction/30);
        GD.Print($"Target: {positionToPush} | Direction: {direction.Normalized()}");
    }
    private Line2D CreateLine()
    {
        
        Line2D line = new Line2D();
        
        line.Width = LineWidth;
        line.Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png");
        line.TextureMode = Line2D.LineTextureMode.Tile; 
        line.TextureFilter = TextureFilterEnum.Nearest;
        
        AddChild(line);
        
        return line;
    }
    
    
    
    
    private void AddSegment()
    {
        RigidBody2D segment = new RigidBody2D();
        // Spawn it slightly offset from the last one to avoid physics overlap
        segment.GlobalPosition = _lastAttachedNode.GlobalPosition + new Vector2(0, DistanceBetweenParts);
    
        segment.Mass = 0.1f;
        segment.CollisionLayer = 4;
        segment.CollisionMask = 1;
    
        // Add CollisionShape (as you did before)
        CollisionShape2D shape = new CollisionShape2D();
        shape.Shape = new CircleShape2D { Radius = 2.0f };
        segment.AddChild(shape);

        AddChild(segment);
        _segments.Add(segment);

        PinJoint2D joint = new PinJoint2D();
        joint.GlobalPosition = _lastAttachedNode.GlobalPosition;
        joint.NodeA = _lastAttachedNode.GetPath();
        joint.NodeB = segment.GetPath();
        joint.Softness = 0.1f;
        AddChild(joint);

        _lastAttachedNode = segment; // The next segment will attach to this one
    }
    private async void GrowTentacle(int length, Vector2 targetPos)
    {
        _targetSegmentCount = length;
        _currentSegmentCount = 0;
        _lastAttachedNode = PlayerNode2D; // Start attaching at the player

        for (int i = 0; i < _targetSegmentCount; i++)
        {
            AddSegment();
            // Wait for 0.05 seconds between each segment
            await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        
            // Optional: Apply a small push toward the mouse as it grows
            PushToward(targetPos);
        }
    }

    public override void _Process(double delta)
    {
        if (_line != null) DisplayLine();
        
        
    }

    private void DisplayLine()
    {
        _line.ClearPoints();
        _line.AddPoint(Vector2.Zero); 
        
        foreach (var segment in _segments)
        {
            _line.AddPoint(ToLocal(segment.GlobalPosition));
        }
    }
    
    
    
    // private Line2D CreateLine(int distanceToObject)
    // {
    //     //300 here is hardcoded because it the distance from an end of the screen to the other
    //     TentaculeLength = distanceToObject / 10;
    //     
    //     Line2D line = new Line2D();
    //     
    //     line.Width = LineWidth;
    //     line.Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png");
    //     
    //     line.TextureMode = Line2D.LineTextureMode.Tile; 
    //
    //     line.TextureFilter = TextureFilterEnum.Nearest;
    //     
    //     
    //     AddChild(line);
    //
    //     
    //     Node2D parentToAttachTo = PlayerNode2D;
    //
    //     for (int i = 0; i < TentaculeLength; i++)
    //     {
    //         RigidBody2D segment = new RigidBody2D();
    //         segment.Position = new Vector2(0, (i + 1) * DistanceBetweenParts);
    //         segment.Mass = 0.1f; // Light segments feel more organic
    //         
    //
    //         
    //         //4 being the tentacule layer
    //         //1 layer on wich the player is
    //         //prevent collision problems between player and tentacles
    //         segment.CollisionLayer = 4;
    //         segment.CollisionMask = 1;
    //         
    //         CollisionShape2D shape = new CollisionShape2D();
    //         shape.Shape = new CircleShape2D { Radius = 2.0f };
    //         segment.AddChild(shape);
    //
    //         AddChild(segment);
    //         _segments.Add(segment);
    //
    //         PinJoint2D joint = new PinJoint2D();
    //         joint.Position = new Vector2(0, i * DistanceBetweenParts);
    //         joint.NodeA = parentToAttachTo.GetPath();
    //         joint.NodeB = segment.GetPath();
    //         joint.DisableCollision = true; 
    //         joint.Softness = 0.1f;       
    //         joint.Bias = 0.1f;             
    //         AddChild(joint);
    //
    //         parentToAttachTo = segment;
    //     }
    //
    //     return line;
    // }
}