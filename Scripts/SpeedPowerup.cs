using Godot;
using System;

public partial class SpeedPowerup : Powerup
{
	[Export]
	public float speedPercentage = 0.40f, duration = 2f;

	public override void GivePowerup(Node body)
	{
		try
		{
			IChar mover = body.GetNode<IChar>(body.GetPath());
			if (mover != null)
			{
				mover.ModifySpeedTimed(speedPercentage, duration);
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

