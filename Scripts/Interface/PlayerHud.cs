using Godot;
using System;
using System.Linq;
using DialogueManagerRuntime;
using Godot.Collections;
using JellyBrain.Scripts.GameLogic;
using JellyBrain.Scripts.Utils;

public partial class PlayerHud : Control
{
    //should be assigned in editor
    [Export]
    public HealthComponent healthComponent;
    
    //For dialogue purpose  
    private RichTextLabel characterNameTextField;
    private RichTextLabel DialogueText;
    private Node2D Particles;


    
    //should contain nodes that manage the display of hearts
    private ColorRect[] hpHeartList;

    public override void _Ready()
    {
        if (healthComponent == null)
        {
            throw new NullReferenceException("Health component was not passed to the node");
        }
        hpHeartList = CollectionsAlternative.GetChildren<ColorRect>(GetNode<VSeparator>("CanvasLayer/AspectRatioContainer/VSeparator"));
        healthComponent.HealthChanged += _onHealthChanged;
        GetNode<GameEvents>("/root/GameEvents").StartDialogue += OnStartDialogueRequest;

    }
    
    
    private void _onHealthChanged(int oldHealth, int newHealth)
    {
        for (int i = 0; i < newHealth; i++)
        {
            hpHeartList[i].SetVisible(true);
        }

        for (int i = newHealth; i < hpHeartList.Length; i++)
        {   
            hpHeartList[i].SetVisible(false);
        }
        //GD.Print($"oldHealth: {oldHealth}, newHealth: {newHealth}");
		
    }
    private void OnStartDialogueRequest(string path, string name)
    {
        DialogueLine dialogue = DialogueManager.GetNextDialogueLine(GD.Load(path), name).GetAwaiter().GetResult();
        if (dialogue != null)
        {
            
        }
    }
    

}
