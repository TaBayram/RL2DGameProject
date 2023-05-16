using Godot;
using System;

public abstract partial class Powerup : Area2D
{
	public abstract void givePowerup(Node body);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node2D body){
		//Is body player or ai?
		GD.Print("hey");
		this.givePowerup(body);

	}
}
