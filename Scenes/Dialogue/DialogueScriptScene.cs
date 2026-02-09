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

    public override void _Process(double delta)
    {
        GD.Print(Scale);
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
        if (CurrentDialoguePath != null && CurrentDialogueName != null && CurrentDialogueFile != null && CurrentLineDisplayed != null)
        {
            //This line gets the next dialogue to follow
            DialogueLine dialogueLine = await DialogueManager.GetNextDialogueLine(CurrentDialogueFile, CurrentLineDisplayed.NextId);
            if (dialogueLine != null)
            {
                readLine(dialogueLine);
            }
        }

    }

    private void readLine(DialogueLine dialogue)
    {
        DialogueText.Text = dialogue.Text;
        DialogueText.VisibleRatio = 0; // Start with text hidden

        Tween textTween = GetTree().CreateTween();
        textTween.TweenProperty(DialogueText, "visible_ratio", 1.0f, dialogue.Text.Length * 0.02f);
        
        
        CurrentLineDisplayed = dialogue;
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
        SetVisible(true);
d
        PopIn();
        
            
        
        readLine(dialogue);
        
    }
    
    private void PopIn()
    {
        Scale = Vector2.Zero; 

        Tween tween = GetTree().CreateTween();
    
        
        tween.TweenProperty(this, "scale", Vector2.One, 0.4f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
         
        GD.Print("Tween started on Node2D");
    }

    private void DialogueEnded(Resource dialogueResource)
    {
        GD.Print("Dialogue ended");
        this.SetVisible(false);
        CurrentDialogueName = null;
        CurrentLineDisplayed = null;
        CurrentDialoguePath = null;
        DialogueText.Text = "";
    }
}
