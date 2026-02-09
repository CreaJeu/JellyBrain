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


    private Resource CurrentDialogueFile;
    private DialogueLine CurrentLineDisplayed;
    private string CurrentDialoguePath;
    private string CurrentDialogueName;

    public override void _Ready()
    {
        characterNameTextField = GetNode<RichTextLabel>("CharacterName");
        Particles = GetNode<Node2D>("Particles");
        DialogueText = GetNode<RichTextLabel>("Text");
        GetNode<GameEvents>("/root/GameEvents").StartDialogue += OnStartDialogueRequest;
        DialogueManager.DialogueEnded += DialogueEnded;

        
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
        if (CurrentDialoguePath != null && CurrentDialogueName != null && CurrentDialogueFile != null)
        {
            //This line gets the next dialogue to follow
            DialogueLine dialogueLine = await DialogueManager.GetNextDialogueLine(CurrentDialogueFile, CurrentLineDisplayed.NextId);
            readLine(dialogueLine);
        }

    }

    private void readLine(DialogueLine dialogue)
    {
        SetVisible(true);
        CurrentLineDisplayed = dialogue;
        DialogueText.Text = dialogue.Text;
        characterNameTextField.Text = dialogue.Character;
        GD.Print("Dialogue open");

        
    }

    public async void OnStartDialogueRequest(string path, string name)
    {
        CurrentDialogueName = name;
        CurrentDialoguePath = path;
        CurrentDialogueFile = GD.Load(path);
        DialogueLine dialogue = await DialogueManager.GetNextDialogueLine(CurrentDialogueFile, name);
        if (dialogue == null)
        {
            throw new IncorrectPathError("Could not find Dialogue line: " + CurrentDialogueName + "\n in path :" +  CurrentDialoguePath);

        }
        readLine(dialogue);
    }

    private void DialogueEnded(Resource dialogueResource)
    {
        this.SetVisible(false);
        CurrentDialogueName = null;
        CurrentLineDisplayed = null;
        CurrentDialoguePath = null;
        DialogueText.Text = "";
    }
}
