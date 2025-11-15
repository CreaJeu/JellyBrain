using Godot;
using System;
using DialogueManagerRuntime;

public partial class GameDialogueManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var dialogue = GD.Load<Resource>("res://Dialogues/test.dialogue");
		DialogueManager.ShowExampleDialogueBalloon(dialogue, "start");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
