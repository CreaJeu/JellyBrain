using Godot;
using System;
using DialogueManagerRuntime;

public partial class DialogueScriptScene : Node2D
{
    private RichTextLabel characterNameTextField;
    private RichTextLabel DialogueText;
    private Node2D Particles;

    public override void _Ready()
    {
        characterNameTextField = GD.Load<RichTextLabel>("CharacterName");
        Particles = GD.Load<Node2D>("Particles");
        DialogueText = GD.Load<RichTextLabel>("Text");
    }
    public void loadAndStartDialogue(string path, string name)
    {
        DialogueLine line = DialogueManager.GetNextDialogueLine(GD.Load(path), name).GetAwaiter().GetResult();
        DialogueText.Text = line.Text;
    }

    public void startDialogue()
    {
        //TODO fill this
    }
}
