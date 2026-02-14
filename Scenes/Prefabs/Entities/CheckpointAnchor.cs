using Godot;
using System;
using JellyBrain.Scripts.GameLogic;

public partial class CheckpointAnchor : Node2D
{
    
    public override void _Ready()
    {
        var interactable = GetNode<Interactable>("Interactable");
        interactable.InteractedWith += _onInteract;

    }

    public void _onInteract()
    {
        GetNode<GameEvents>("/root/GameEvents").EmitSignal(GameEvents.SignalName.SaveGameAndPosition,
            this.Position,"first anchor");

    }
}
