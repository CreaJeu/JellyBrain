using Godot;
using System;

public partial class HealthComponent : Node2D
{
	[Export] private int _maxHealth = 5;
	private int _currentHealth;

	[Signal] public delegate void HealthChangedEventHandler(int oldHealth, int newHealth);
	[Signal] public delegate void HealthBelowZeroEventHandler(int newHealth);
	
	public override void _Ready()
	{
		_currentHealth = _maxHealth;
	}

	public void ApplyDamage(int damage)
	{
		if (_currentHealth <= 0) return;
		
		EmitSignalHealthChanged(_currentHealth, _currentHealth-damage);
		_currentHealth -= damage;
		if (_currentHealth <= 0)
		{
			EmitSignalHealthBelowZero(_currentHealth);
		}
	}

}
