using Godot;

namespace JellyBrain.Scripts.GameLogic;

public partial class GameEvents : Node
{
    /**
     * Used in DialogueScriptScene.cs and emitted from StaticNpc.cs
     */
    [Signal]
    public delegate void StartDialogueEventHandler(string path, string name);
    
    /**
     * Used in SavingIcon.cs and emitted from CheckPointAnchor.cs
     */
    [Signal]
    public delegate void SaveGameAndPositionEventHandler(Vector2  position, string name);
    
    /**
     * emitted in DialogueScriptScene.cs and recieved in Player.cs
     */
    [Signal]
    public delegate void PausePlayerInteractionsEventHandler(bool  pause);

}