extends Camera

var look_at_point
var offset
var offset_dir
var offset_set = false

func _ready():
	offset = Vector3(0, 40, 5)
	look_at_point = Vector3.ZERO
	offset_dir = Vector3.ZERO

func _physics_process(delta):
	if !offset_set and look_at_point != Vector3.ZERO:
		offset_dir = look_at_point - shoot_ray()
		offset_set = true;
	offset_dir.y = 0
	global_transform.origin = look_at_point + offset + offset_dir
	
func set_look_at_point(point):
	look_at_point = point
	look_at_point.y = 0
	
func shoot_ray():
	var from = global_transform.origin
	var to = from - global_transform.basis.z * 1000
	var hit_point = get_world().direct_space_state.intersect_ray(from, to, [self]).position
	hit_point.y = 0
	return hit_point

