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

    private DialogueLine currentLineDisplayed;
    private string CurrentDialoguePath;
    private string CurrentDialogueName;

    public override void _Ready()
    {
        characterNameTextField = GetNode<RichTextLabel>("CharacterName");
        Particles = GetNode<Node2D>("Particles");
        DialogueText = GetNode<RichTextLabel>("Text");
        GetNode<GameEvents>("/root/GameEvents").StartDialogue += OnStartDialogueRequest;

    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event.IsActionPressed("skip_dialogue"))
        {
            nextLine();
            GetViewport().SetInputAsHandled(); 
            GD.Print("Skip dialogue");
        }
    }

    private async void nextLine()
    {
        if (CurrentDialoguePath != null && CurrentDialogueName != null)
        {
            DialogueLine dialogue = await DialogueManager.GetNextDialogueLine(GD.Load(CurrentDialoguePath), CurrentDialogueName);
            readLine(dialogue);
        }

    }

    private void readLine(DialogueLine dialogue)
    {
        if (dialogue != null)
        {
            SetVisible(true);
            currentLineDisplayed = dialogue;
            DialogueText.Text = dialogue.Text;
            GD.Print("Dialogue open");

        }
        else
        {
            throw new IncorrectPathError("Could not find Dialogue line: " + CurrentDialogueName + "\n in path :" +  CurrentDialoguePath);

        }
    }

    public async void OnStartDialogueRequest(string path, string name)
    {
        CurrentDialogueName = name;
        CurrentDialoguePath = path;
        DialogueLine dialogue = await DialogueManager.GetNextDialogueLine(GD.Load(path), name);
        readLine(dialogue);
    }
}
