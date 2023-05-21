using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D, IChar
{
    float speed = 200.0f;
    Vector2 startPosition;

    [Export]
    public float Speed { get => speed; set => speed = value; }

    public bool IsPlayer => true;


    public override void _Ready()
    {
        startPosition = GlobalPosition;
    }

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

    public void Reset()
    {
        GlobalPosition = startPosition;
    }
}
