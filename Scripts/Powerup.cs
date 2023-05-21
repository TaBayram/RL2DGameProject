using Godot;
using System;

public abstract partial class Powerup : Area2D
{
	[Export]
	public string name;
	public bool isUsed = false;
	public abstract void GivePowerup(Node body);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node2D body){
		this.GivePowerup(body);
	}

	protected void Destroy(){
		isUsed = true;
		QueueFree();
	}
}
