using Godot;
using System;

public partial class TestObject : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var interactable = GetNode<Interactable>("Interactable");
		interactable.InteractedWith += _onInteract;
	}

	private void _onInteract()
	{
		GD.Print($"{Name} Received interact and can do stuff!");
	}
}
