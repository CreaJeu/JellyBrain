using Godot;
using System;
using System.Collections.Generic;

public partial class Tentacules : Node2D
{
    [Export] public int TentaculeLength = 10;
    [Export] public float DistanceBetweenParts = 6.0f;
    [Export] public float LineWidth = 2.0f; 
    [Export] public Node2D PlayerNode2D;
    
    
    
    private List<RigidBody2D> _segments = new List<RigidBody2D>();
    private Line2D _line;

    public override void _Ready()
    {
        _line = CreateLine(150);

    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("grab"))
        {
        //TODO do shit
        }
    }

    private Line2D CreateLine(int distanceToObject)
    {
        //300 here is hardcoded because it the distance from an end of the screen to the other
        TentaculeLength = distanceToObject / 10;
        
        Line2D line = new Line2D();
        
        line.Width = LineWidth;
        line.Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png");
        
        line.TextureMode = Line2D.LineTextureMode.Tile; 
    
        line.TextureFilter = TextureFilterEnum.Nearest;
        
        
        AddChild(line);

        
        Node2D parentToAttachTo = PlayerNode2D;

        for (int i = 0; i < TentaculeLength; i++)
        {
            RigidBody2D segment = new RigidBody2D();
            segment.Position = new Vector2(0, (i + 1) * DistanceBetweenParts);
            segment.Mass = 0.1f; // Light segments feel more organic
            

            
            //4 being the tentacule layer
            //1 layer on wich the player is
            //prevent collision problems between player and tentacles
            segment.CollisionLayer = 4;
            segment.CollisionMask = 1;
            
            CollisionShape2D shape = new CollisionShape2D();
            shape.Shape = new CircleShape2D { Radius = 2.0f };
            segment.AddChild(shape);

            AddChild(segment);
            _segments.Add(segment);

            PinJoint2D joint = new PinJoint2D();
            joint.Position = new Vector2(0, i * DistanceBetweenParts);
            joint.NodeA = parentToAttachTo.GetPath();
            joint.NodeB = segment.GetPath();
            joint.DisableCollision = true; 
            joint.Softness = 0.1f;       
            joint.Bias = 0.1f;             
            AddChild(joint);

            parentToAttachTo = segment;
        }

        return line;
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
}