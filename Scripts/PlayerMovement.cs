using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D, IMover
{
	float movementSpeed = 200.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public float Speed { get => movementSpeed; set => movementSpeed = value; }

	public async void ModifySpeedTimed(float scale, float duration)
	{
		float modifiedSpeed = Speed * scale;
		Speed += modifiedSpeed;

		await ToSignal(GetTree().CreateTimer(duration), "timeout");
		Speed -= modifiedSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
