using Godot;
using System;

public partial class DeathPowerup : Powerup
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GivePowerup(Node body)
	{
		CharacterBody2D cBody = body.GetNode<CharacterBody2D>(body.GetPath());
		if (cBody != null)
		{
			ScoreManager.instance.ResetPlayerPoints();
			this.Destroy();
		}
	}


	private void _on_timer_timeout()
	{
		this.Destroy();
		// Replace with function body.
	}

}
