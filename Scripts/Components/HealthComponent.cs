
using Godot;

public partial class HealthComponent : Node2D
{
	[Export] 
	public int MaxHealthPoints { get; set; } = 10;

	public int HealthPoints;

	public HealthComponent()
	{
		this.HealthPoints = MaxHealthPoints;
	}
	public void DamageTaken(AttackComponent attackComponent)
	{
		this.HealthPoints -= attackComponent.AttackDamage;
		if(HealthPoints <= 0) GetParent().QueueFree();
	}

}
