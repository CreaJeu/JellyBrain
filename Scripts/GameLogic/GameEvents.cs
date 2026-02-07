using Godot;

namespace JellyBrain.Scripts.GameLogic;

public partial class GameEvents : Node
{
    [Signal]
    public delegate void StartDialogueEventHandler(string path, string name);
}