using Godot;
using System;
using System.Collections.Generic;


public partial class PowerupSpawner : Node2D
{
	[Export]
	Node2D topLeft, botRight;
	[Export]
	PackedScene[] powerups;
	[Export]
	int[] chances;

	RandomNumberGenerator randomNumberGenerator;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		randomNumberGenerator = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _on_timer_timeout()
	{
		int chance = randomNumberGenerator.RandiRange(0, 100);
		GD.Print(chance);
		PackedScene packedPowerup = null; 
		for (int i = 0; i < powerups.Length; i++)
		{
			if (chances[i] >= chance)
			{
				packedPowerup = powerups[i];
				break;
			}
		}
		if (packedPowerup != null)
		{
			Node2D powerup = packedPowerup.Instantiate<Node2D>();
			AddChild(powerup);
			powerup.Position = GetRandomPosition();
		}
	}

	private Vector2 GetRandomPosition()
	{
		return new Vector2(randomNumberGenerator.RandfRange(topLeft.Position.X, botRight.Position.X),
							randomNumberGenerator.RandfRange(topLeft.Position.Y, botRight.Position.Y));
	}
}


