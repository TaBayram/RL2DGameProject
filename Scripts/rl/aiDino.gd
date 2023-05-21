extends CharacterBody2D

@onready var ball = get_node("../Player")
@onready var ai_controller: AIDinoController = $AIController2D
const SPEED = 200.0

var startPosition: Vector2

func _ready():
	startPosition = global_position
	ai_controller.init(self)


func _physics_process(delta):
	if ai_controller.needs_reset:
		ai_controller.reset()
		return
		
	var direction = ai_controller.move_action
	if direction.length() > 1.0:
		direction = direction.normalized()
	velocity = direction * SPEED
	move_and_slide()
	
	ai_controller.update_reward()
	
			
	if Input.is_action_just_pressed("r_key"):
		ai_controller.reset()
	

func game_over():
	ai_controller.done = true
	ai_controller.needs_reset = true

func _on_area_2d_area_entered(area):
	ai_controller.reward += 10
	game_over()


func _on_ai_controller_2d_on_reset():
	velocity = Vector2.ZERO
	global_position = startPosition
