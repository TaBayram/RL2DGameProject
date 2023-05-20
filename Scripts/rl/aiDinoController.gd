extends AIController2D
class_name AIDinoController
#Signals
signal onReset

# Stores the action sampled for the agent's policy, running in python
var move_action = Vector2.ZERO
var bestBallDistance = 999999

#get observations. Basically passing world information. AI uses it 
func get_obs() -> Dictionary:
	# get the balls position and velocity in the paddle's frame of reference
	var relative = _player.ball.position - _player.position
	var distance = relative.length() / 1500.0 
	relative = relative.normalized() 
	
	
	var result := []
	result.append(relative.x)
	result.append(relative.y)
	result.append(distance)

	
	return {"obs":result}

func get_reward() -> float:	
	return reward
	
func get_action_space() -> Dictionary:
	return {
		"move" : {
		"size": 2,	
			"action_type": "continuous"
 }
}
	
func set_action(action) -> void:	
	move_action.x = action["move"][0]
	move_action.y = action["move"][1]

func reset():
	bestBallDistance = 999999
	n_steps = 0
	needs_reset = false
	onReset.emit();

func update_reward():
	reward -= 0.005 # step penalty
	reward += shaping_reward()
	
func shaping_reward():
	var s_reward = 0.0
	var ballDistance = _player.position.distance_to(_player.ball.position)
	
	if ballDistance < bestBallDistance:
		s_reward += bestBallDistance - ballDistance
		#print("Best",bestBallDistance," now ",ballDistance, " reward ", reward)
		bestBallDistance = ballDistance
		
	s_reward /= 10.0
	return s_reward
