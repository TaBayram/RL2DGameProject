public interface IMover 
{
	public float Speed { get; set; }
	public void ModifySpeedTimed(float scale, float duration);
}
