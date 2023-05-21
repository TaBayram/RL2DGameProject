using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	public PowerupSpawner powerupSpawner;
	[Export]
	public ScoreManager scoreManager;
	[Export]
	public PlayerMovement player;
	[Export]
	public AIYDino ai;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
