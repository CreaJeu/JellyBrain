using Godot;
using System;
using DialogueManagerRuntime;
using JellyBrain.Scripts.Errors;
using JellyBrain.Scripts.GameLogic;

public partial class DialogueScriptScene : Node2D
{
    private RichTextLabel characterNameTextField;
    private RichTextLabel DialogueText;
    private Node2D Particles;

    public override void _Ready()
    {
        characterNameTextField = GetNode<RichTextLabel>("CharacterName");
        Particles = GetNode<Node2D>("Particles");
        DialogueText = GetNode<RichTextLabel>("Text");
        GetNode<GameEvents>("/root/GameEvents").StartDialogue += OnStartDialogueRequest;

    }
    
    private async void OnStartDialogueRequest(string path, string name)
    {
        DialogueLine dialogue = await DialogueManager.GetNextDialogueLine(GD.Load(path), name);
        if (dialogue != null)
        {
            SetVisible(true);
            DialogueText.Text = dialogue.Text;
            GD.Print("Dialogue open");

        }
        else
        {
            throw new IncorrectPathError("Could not find Dialogue line: " + name);
        }
    }
}
