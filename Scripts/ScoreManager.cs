using Godot;

public partial class ScoreManager : Node2D
{
    public static ScoreManager instance;
    [Export]
    Label playerLabel, aiLabel;
    float playerScore = 0, aiScore = 0;

    public float PlayerScore { get => playerScore; set { playerScore = value; playerLabel.Text = "Player: " + PlayerScore;} }
    public float AiScore { get => aiScore; set { aiScore = value; aiLabel.Text = "AI: " + AiScore; } }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScoreManager.instance = this;

        ResetPlayerPoints();
        ResetAIPoints();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void GivePointToPlayer()
    {
        PlayerScore++;
    }

    public void GivePointToAI()
    {
        AiScore++;

    }

    public void ResetPlayerPoints()
    {
        PlayerScore = 0;
    }

    public void ResetAIPoints()
    {
        AiScore = 0;
    }
}
