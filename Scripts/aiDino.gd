extends CharacterBody2D

@export var rotation_speed = 3.0
@onready var ball = get_node("../Ball")
@onready var ball2 = get_node("../Ball2")
@onready var ai_controller = $AIController2D
const SPEED = 200.0


func _ready():
	ai_controller.init(self)


func _physics_process(delta):
	if ai_controller.needs_reset:
		ai_controller.reset()
		return
		
	var direction = ai_controller.move_action
	velocity = direction * SPEED
	move_and_slide()
	
	ai_controller.update_reward()
	

func game_over():
	ai_controller.done = true
	ai_controller.needs_reset = true

func _on_area_2d_area_entered(area):
	print("REWARDED")
	ai_controller.reward += 100.0
