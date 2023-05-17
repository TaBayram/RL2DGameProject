using Godot;
using System;

public partial class SpeedPowerup : Powerup
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
		IMover mover = body.GetNode<IMover>(body.GetPath());
		if(mover != null){
			mover.ModifySpeedTimed(0.20f, 10);
			this.Destroy();
		}
	}
	
	
private void _on_timer_timeout()
{
	this.Destroy();
	// Replace with function body.
}

}

