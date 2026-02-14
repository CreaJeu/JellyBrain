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
        _line = new Line2D();
        _line.Width = LineWidth;
        _line.Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png");
        
        _line.TextureMode = Line2D.LineTextureMode.Tile; 
    
        _line.TextureFilter = TextureFilterEnum.Nearest;
        
        
        AddChild(_line);

        
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
            shape.Shape = new CircleShape2D { Radius = 5.0f };
            segment.AddChild(shape);

            AddChild(segment);
            _segments.Add(segment);

            PinJoint2D joint = new PinJoint2D();
            joint.Position = new Vector2(0, i * DistanceBetweenParts);
            joint.NodeA = parentToAttachTo.GetPath();
            joint.NodeB = segment.GetPath();
            joint.DisableCollision = true; 
            joint.Softness = 1.5f;       
            joint.Bias = 0.1f;             
            AddChild(joint);

            parentToAttachTo = segment;
        }
    }

    public override void _Process(double delta)
    {
        _line.ClearPoints();
        
        _line.AddPoint(Vector2.Zero); 

        foreach (var segment in _segments)
        {
            _line.AddPoint(ToLocal(segment.GlobalPosition));
        }
    }
}