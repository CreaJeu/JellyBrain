using Godot;
using System;

public partial class StickyEnemyPath : Path2D
{
	private PathFollow2D pathFollow;

	private const float enemySpeed = 20.0f;

	public override void _Ready() {
		pathFollow = GetNode<PathFollow2D>("PathFollow2D");
	}

	public override void _PhysicsProcess(double delta) {
		pathFollow.Progress += (float) delta * enemySpeed;
	}
}
