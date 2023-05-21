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
		try
		{
			IChar character = body.GetNodeOrNull<IChar>(body.GetPath());
			if (character != null)
			{
				character.Reset();
				if(character.IsPlayer){
					ScoreManager.instance.ResetPlayerPoints();
				}
				else{
					ScoreManager.instance.ResetAIPoints();
				}
				this.Destroy();
			}

		}
		catch (InvalidCastException castException)
		{

		}
	}


	private void _on_timer_timeout()
	{
		this.Destroy();
		// Replace with function body.
	}

}
