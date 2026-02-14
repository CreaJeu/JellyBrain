using Godot;

namespace JellyBrain.Scripts.GameLogic;

public partial class GameEvents : Node
{
    [Signal]
    public delegate void StartDialogueEventHandler(string path, string name);
    [Signal]
    public delegate void SaveGameAndPositionEventHandler(Vector2  position, string name);
}