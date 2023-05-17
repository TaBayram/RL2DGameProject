using Godot;
using System;

public partial class ScoreManager : Node2D
{
	public static ScoreManager instance;
	[Export]
	Label playerLabel;
	float playerScore = 0, enemyScore = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreManager.instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GivePointToPlayer()
	{
		playerScore++;
		playerLabel.Text = "Score: " + playerScore;
	}

	public void ResetPlayerPoints(){
		playerScore = 0;
		playerLabel.Text = "Score: " + playerScore;
	}
}
