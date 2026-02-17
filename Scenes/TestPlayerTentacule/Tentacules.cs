using Godot;
using System.Collections.Generic;

public partial class Tentacules : Node2D
{
    [Export] public float SegmentSize = 4.0f;
    [Export] public int MaxSegments = 50;
    [Export] public Node2D PlayerNode2D;
    [Export(PropertyHint.Layers2DPhysics)] public uint CollisionMask = 1;

    private List<Vector2> _segmentPositions = new List<Vector2>();
    private Line2D _line;
    private GrabbableExampleObject _grabbedObject = null;

    public override void _Ready()
    {
        _line = new Line2D {
            Width = SegmentSize,
            TextureMode = Line2D.LineTextureMode.Tile,
            TextureFilter = TextureFilterEnum.Nearest,
            Texture = GD.Load<Texture2D>("res://Assets/Sprites/Neutral/tentaculePart.png"),
            Antialiased = false
        };
        AddChild(_line);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (PlayerNode2D == null) return;

        if (Input.IsActionPressed("grab"))
        {
            // The tip is the Target (Mouse)
            Vector2 target = GetGlobalMousePosition();
            UpdateTentacle(target);
            HandleGrabbing(delta);
        }
        else
        {
            ReleaseObject();
            _segmentPositions.Clear();
            _line.ClearPoints();
        }
    }

    private void UpdateTentacle(Vector2 targetPos)
    {
        _segmentPositions.Clear();
        _line.ClearPoints();

        Vector2 start = PlayerNode2D.GlobalPosition;
        Vector2 direction = (targetPos - start).Normalized();
        float dist = start.DistanceTo(targetPos);
        
        // Ensure we always have at least 1 segment
        int count = Mathf.Clamp((int)(dist / SegmentSize), 1, MaxSegments);

        var spaceState = GetWorld2D().DirectSpaceState;
        Vector2 currentPoint = start;

        // Add the start point (Player)
        _line.AddPoint(_line.ToLocal(currentPoint));
        _segmentPositions.Add(currentPoint);

        for (int i = 0; i < count; i++)
        {
            Vector2 nextPoint = currentPoint + direction * SegmentSize;

            // Collision check
            var query = PhysicsRayQueryParameters2D.Create(currentPoint, nextPoint, CollisionMask);
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                Vector2 hitPos = (Vector2)result["position"];
                _segmentPositions.Add(hitPos);
                _line.AddPoint(_line.ToLocal(hitPos));
                break; // Stop tentacle at the wall
            }

            _segmentPositions.Add(nextPoint);
            _line.AddPoint(_line.ToLocal(nextPoint));
            currentPoint = nextPoint;
        }
    }

    private void HandleGrabbing(double delta)
    {
        if (_segmentPositions.Count == 0) return;
        
        // The tip is the last point in our calculated list
        Vector2 tip = _segmentPositions[^1];

        if (_grabbedObject == null)
        {
            // Only look for a NEW object if we aren't already holding one
            foreach (Node node in GetTree().GetNodesInGroup("grabbable"))
            {
                if (node is GrabbableExampleObject obj && obj.GlobalPosition.DistanceTo(tip) < SegmentSize * 4.0f)
                {
                    _grabbedObject = obj;
                    _grabbedObject.OnGrab();
                    break;
                }
            }
        }
        else
        {
            // If we ARE holding one, force it to the tip position
            _grabbedObject.MoveTo(tip, delta);
        }
    }

    private void ReleaseObject()
    {
        if (_grabbedObject != null)
        {
            _grabbedObject.OnRelease(Vector2.Zero);
            _grabbedObject = null;
        }
    }
}