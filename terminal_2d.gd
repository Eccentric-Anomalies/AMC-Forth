## Terminal 2D AMC Forth Demo
extends Node2D

# Test signals FIXME
signal port_99(value: int)
signal input_100(value: int)

var _telnet_terminal: ForthTermTelnet
var _local_terminal: ForthTermLocal

var _forth: AMCForth


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
	_local_terminal = ForthTermLocal.new(_forth, $Bezel/Screen.material)

	# outputs
	_forth.AddOutputSignal(99, port_99)  # FIXME test purposes
	port_99.connect(_on_port_99_output)  # FIXME output test

	# inputs
	_forth.AddInputSignal(100, input_100)  # FIXME test input


# Clean up when closing app
func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		_forth.Cleanup()
		get_tree().quit()  # default


func _process(_delta: float) -> void:
	# perform periodic telnet processing
	_telnet_terminal.poll_connection()
	_local_terminal.update_time()


func _unhandled_key_input(evt: InputEvent) -> void:
	_local_terminal.handle_key_event(evt)
	get_viewport().set_input_as_handled()


# output test FIXME
func _on_port_99_output(value: int):
	print(value)


# test code FIXME
func _input(event):
	if event is InputEventKey:
		if event.pressed and event.keycode == KEY_SPACE:
			input_100.emit(666)
