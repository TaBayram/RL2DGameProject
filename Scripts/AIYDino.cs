using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class AIYDino : CharacterBody2D, IChar
{
	Vector2 startPosition;
	private float speed = 100;

	[Export]
	public float Speed { get => speed; set { speed = value; if(agent2D != null) agent2D.MaxSpeed = speed; } }

	public bool IsPlayer => false;

	public NavigationAgent2D agent2D;

	public override void _Ready()
	{
		startPosition = GlobalPosition;

		agent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
		agent2D.MaxSpeed = Speed;
		agent2D.VelocityComputed += _on_navigation_agent_velocity_computed;
		agent2D.NavigationFinished += onNavigationAgentTargetReached;

		GetNode<Area2D>("Area2DAI").AreaEntered += onAreaEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!agent2D.IsNavigationFinished())
		{
			var targetGlobalPosition = agent2D.GetNextPathPosition();
			var direction = GlobalPosition.DirectionTo(targetGlobalPosition);
			var desiredVelocity = direction.Normalized() * Speed;
			agent2D.SetVelocity(desiredVelocity);
		}

		AiProcess(delta);
	}

	public async void ModifySpeedTimed(float scale, float duration)
	{
		float modifiedSpeed = Speed * scale;
		Speed += modifiedSpeed;

		await ToSignal(GetTree().CreateTimer(duration), "timeout");
		Speed -= modifiedSpeed;
	}

	//Section: Ai stuff
	[Export]
	public double thinkInterval = 1;
	[Export]
	public double thinkAfterFinish = 0.10;
	private double thinkingCooldown = 0;
	private Powerup targetPowerup;
	private List<LearnData> learnDatas = new List<LearnData>();


	private void AiProcess(double delta)
	{
		if (targetPowerup != null && targetPowerup.isUsed)
		{
			targetPowerup = null;
			thinkingCooldown = 0;
		}

		thinkingCooldown -= delta;
		if (thinkingCooldown > 0)
		{
			return;
		}
		Array<Node> nodes = GetTree().GetNodesInGroup("Powerup");
		if (nodes.Count == 0)
		{
			return;
		}
		Array<Powerup> powerups = new Array<Powerup>();
		foreach (Node node in nodes)
		{
			var powerup = node.GetNode<Powerup>(".");
			if (powerup != null)
			{
				powerups.Add(powerup);
			}
		}
		ChoosePowerup(powerups);

		if (targetPowerup != null)
		{
			thinkingCooldown = thinkInterval;
		}
	}


	private void ChoosePowerup(Array<Powerup> powerups)
	{

		powerups = GetNearestUniquePowerups(powerups);

		double candidateScore = double.MinValue;

		foreach (Powerup powerup in powerups)
		{
			LearnData data = learnDatas.Find((LearnData cData) => cData.id == powerup.name);
			if (data == null)
			{
				data = new LearnData();
				data.id = powerup.name;
				data.score = 0;
				learnDatas.Add(data);
			}
			if (data.score < 0)
			{
				var obstacle = powerup.GetNodeOrNull<NavigationObstacle2D>("obstacle");
				if (obstacle == null)
				{
					obstacle = new NavigationObstacle2D();
					obstacle.Name = "obstacle";
					obstacle.Radius = 20;
					powerup.AddChild(obstacle);
				}

			}
			if(data.isUnkown){
				targetPowerup = powerup;
				candidateScore = data.score;
				break;
			}
			if (data.score > candidateScore && data.score >= 0)
			{
				targetPowerup = powerup;
				candidateScore = data.score;
			}
		}

		if (targetPowerup != null)
		{
			agent2D.TargetPosition = targetPowerup.GlobalPosition;
		}

	}

	private Array<Powerup> GetNearestUniquePowerups(Array<Powerup> powerups)
	{
		Array<Powerup> uniques = new Array<Powerup>();
		Array<double> distances = new Array<double>();
		foreach (Powerup powerup in powerups)
		{
			bool exists = false;
			double distance = GlobalPosition.DistanceSquaredTo(powerup.GlobalPosition); ;
			for (int i = uniques.Count - 1; i >= 0; i--)
			{
				if (powerup.name == uniques[i].name)
				{
					exists = true;
					if (distance < distances[i])
					{
						uniques[i] = powerup;
						distances[i] = distance;
					}
					break;
				}
			}
			if (!exists)
			{
				uniques.Add(powerup);
				distances.Add(distance);
			}
		}

		return uniques;
	}


	private void _on_navigation_agent_velocity_computed(Vector2 safe_velocity)
	{
		Velocity = safe_velocity;
		MoveAndSlide();
	}


	private void onNavigationAgentTargetReached()
	{
		if (thinkingCooldown < thinkAfterFinish)
		{
			thinkingCooldown = thinkAfterFinish;
		}
	}


	private void onAreaEntered(Area2D area)
	{
		Powerup powerup = GetNodeOrNull<Powerup>(area.GetPath());
		if (powerup != null)
		{
			double score = 0;
			switch (powerup.name)
			{
				case "Speed":
					score = 0.2;
					break;
				case "Point":
					score = 1;
					break;
				case "Death":
					score = -1;
					break;
			}

			var index = learnDatas.FindIndex((data) => data.id == powerup.name);
			if (index != -1)
			{
				learnDatas[index].score = score;
				learnDatas[index].isUnkown = false;
			}
		}
	}

	public void Reset()
	{
		targetPowerup = null;
		thinkingCooldown = 0;
		GlobalPosition = startPosition;
	}


}



class LearnData
{
	public string id;
	public double score;
	public bool isUnkown;

	public LearnData()
	{
		id = "";
		isUnkown = true;
	}
}

struct PowerupWithDistance
{
	public Powerup powerup;
	public double distance;
}
