## Terminal 3D AMC Forth Demo
extends Node3D

var _telnet_terminal: ForthTermTelnet
var _local_terminal: ForthTermLocal

var _forth: AMCForth


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	_forth = AMCForth.new()
	_forth.Initialize(self)
	# When executing in the Godot IDE,
	# Generate Documentation File and
	# Generate Test Suite Output
	if OS.has_feature("editor"):
		add_child(Test.new())
		add_child(Docs.new())
	_telnet_terminal = ForthTermTelnet.new(_forth)
	_local_terminal = ForthTermLocal.new(_forth, $Bezel/Screen.mesh.material)


# Clean up when closing app
func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		_forth.Cleanup()
		get_tree().quit()  # default


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	# perform periodic telnet processing
	_telnet_terminal.poll_connection()
	_local_terminal.update_time()


func _unhandled_key_input(evt: InputEvent) -> void:
	_local_terminal.handle_key_event(evt)
	get_viewport().set_input_as_handled()
