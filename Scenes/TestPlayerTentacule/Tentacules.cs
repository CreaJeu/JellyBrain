using Godot;
using System;
using System.Collections.Generic;

public partial class Tentacules : Node2D
{
    [Export] public float DistanceBetweenParts = 6.0f;
    [Export] public float LineWidth = 2.0f; 
    [Export] public float PushStrength =  1.0f;
    [Export] public float ForceThatPushTentaculeToMouse = 3.0f ;


    [Export] public float JointsSoftness = 0.1f;
    [Export] public float JointsBias = 0.1f;
    
    
    [Export] public float TimeBetweenSpawn = 0.025f;
    [Export] public Node2D PlayerNode2D;
    
    
    private List<RigidBody2D> _segments = new List<RigidBody2D>();
    private Line2D _line;
    private int _targetSegmentCount = 10;
    private Node2D _lastAttachedNode;
    private bool _currentlyGrabbing = false;

    private int _ropeSegmentMass = 50;

    public override void _Ready()
    {
        _line = CreateLine();
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("grab"))
        {
            foreach (var s in _segments) s.QueueFree();
            _segments.Clear();
            Vector2  DistanceToReach = this.GlobalPosition - GetGlobalMousePosition();
            _currentlyGrabbing = true;
            
            //hardcoded 5 Value it will probably need a change if the viewport size changes or if size of things changes
            int tentaculeLength = (int)DistanceToReach.Length() / 5;
             GrowTentacle(tentaculeLength,GetGlobalMousePosition());
        }

        if (@event.IsActionReleased("grab"))
        {
            foreach (var s in _segments) s.QueueFree();
            _segments.Clear();
            _currentlyGrabbing = false;
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
    
    
    
    
    private void AddSegment(int i)
    {
        RigidBody2D segment = new RigidBody2D();
        segment.Position = new Vector2(0, (i + 1) * DistanceBetweenParts);
        if (i == 0) segment.Mass = _ropeSegmentMass;
        
        //will break  if i is in a loop that is not going from 0 to X
        else segment.Mass = _ropeSegmentMass /=  2 ; // Light segments feel more organic
        
    
            
        //4 being the tentacule layer
        //1 layer on wich the player is
        //prevent collision problems between player and tentacles
        segment.CollisionLayer = 4;
        segment.CollisionMask = 1;
            
        CollisionShape2D shape = new CollisionShape2D();
        shape.Shape = new CircleShape2D { Radius = DistanceBetweenParts/2 };
        segment.AddChild(shape);
    
        AddChild(segment);
        _segments.Add(segment);
    
        PinJoint2D joint = new PinJoint2D();
        joint.Position = new Vector2(0, i * DistanceBetweenParts);
        joint.NodeA = _lastAttachedNode.GetPath();
        joint.NodeB = segment.GetPath();
        joint.DisableCollision = true; 
        joint.Softness = JointsSoftness;       
        joint.Bias = JointsBias ;             
        AddChild(joint);
    
        _lastAttachedNode = segment; // The next segment will attach to this one
    }
    private async void GrowTentacle(int length, Vector2 targetPos)
    {
        
        LookAt(targetPos);
        Rotation -= Mathf.Pi / 2;
        
        _targetSegmentCount = length;
        _lastAttachedNode = PlayerNode2D;

        for (int i = 0; i < _targetSegmentCount; i++)
        {
            AddSegment(i);
            // Wait for 0.05 seconds between each segment
            await ToSignal(GetTree().CreateTimer(TimeBetweenSpawn), "timeout");
        
            // Optional: Apply a small push toward the mouse as it grows
            PushToward(targetPos);
        }

        foreach (var segment in _segments)
        {
            GD.Print(segment.Mass);
        }
        {
            
        }
    }

    public override void _Process(double delta)
    {
        if (_line != null) DisplayLine();
        
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_line != null && _currentlyGrabbing)
        {
            ConstantPushTowardMouse();
        }
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

    private void ConstantPushTowardMouse()
    {
        for (int i = 0; i < _segments.Count ; i++)
        {

            RigidBody2D segment = _segments[i];
            // Vector2  distanceToReach = this.GlobalPosition - GetGlobalMousePosition();
                
            Vector2 direction = GetGlobalMousePosition() - segment.GlobalPosition;
        
            // resets it each frame so that when the mouse moves rigid bodies move too
            //TODO maybe improve this later on ? 
            segment.ConstantForce = direction * ForceThatPushTentaculeToMouse;
            if (i == _targetSegmentCount)
            {
                segment.ConstantForce = direction * ForceThatPushTentaculeToMouse * 2;
            }

        }
    }
    
    
    
    // private Line2D CreateLineAncient(int distanceToObject)
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