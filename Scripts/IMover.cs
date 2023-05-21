public interface IChar
{
	public bool IsPlayer { get; }
	public float Speed { get; set; }
	public void ModifySpeedTimed(float scale, float duration);
	public void Reset();
}
