using Godot;
using System;
using JellyBrain.Scripts.GameLogic;

public partial class SavingIcon : Node2D
{
    public override void _Ready()
    {
        // Make sure the node starts invisible
        Modulate = new Color(1, 1, 1, 0); 
        GetNode<GameEvents>("/root/GameEvents").SaveGameAndPosition += LaunchSaving;
    }

    private void LaunchSaving(Vector2 position, string name)
    {
        this.SetVisible(true);
        var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play();

        Tween tween = CreateTween();

        tween.TweenProperty(this, "modulate:a", 1.0f, 1.0f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);

        tween.TweenInterval(0.5f);

        tween.TweenProperty(this, "modulate:a", 0.0f, 2.0f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.In);
             
        tween.Finished += () => sprite.Stop();
    }
}