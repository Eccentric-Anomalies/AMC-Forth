class_name ForthTermLocal
## Local Forth terminal
##

extends ForthTermBase

# bitmasks for different display modes in the terminal
# (must match definitions in shader)
const BOLD_MASK := 0x0100
const LOWINTENSITY := 0x0200
const UNDERLINE_MASK := 0x0400
const BLINK_MASK := 0x0800
const REVERSE_MASK := 0x1000
const INVISIBLE_MASK := 0x2000
const ASCII_MASK := 0x007f

const CURSOR_PERIOD_MSEC := 1000

var _us_key_map: Dictionary

var _screen_ram: PackedInt32Array

var _cursor: Vector2i = Vector2i(1, 1)
var _save_cursor: Vector2i = Vector2i(1, 1)
var _mode: int = 0
var _save_mode: int = 0
var _screen_material: ShaderMaterial
var _last_msec: int = 0
var _cursor_state: bool = false
var _blink_state: bool = false

# Special characters and combos
var _sp_chars: Dictionary

# a list of keys, sorted in reverse order by length
var _sp_chars_keys: Array = []

var _blank


## Initialize special character handler jump table
func _init_handlers() -> void:
	_sp_chars = {
		BSP: _do_bsp,
		CR_CHAR: _do_cr,
		LF: _do_lf,
		DEL_LEFT: _do_del_left,
		DEL: _do_del,
		UP: _do_up,
		DOWN: _do_down,
		RIGHT: _do_right,
		LEFT: _do_left,
		CLRLINE: _do_clrline,
		CLRSCR: _do_clrscr,
		PUSHXY: _do_pushxy,
		POPXY: _do_popxy,
		ESC: _do_esc,
		MODESOFF: _do_modesoff,
		BOLD: _do_bold,
		LOWINT: _do_lowint,
		UNDERLINE: _do_underline,
		BLINK: _do_blink,
		REVERSE: _do_reverse,
		INVISIBLE: _do_invisible,
		ATXY_START: _do_atxy_start,
	}


## Initialize the terminal key map
func _init_keymaps() -> void:
	_us_key_map = {
		"QuoteLeft": "`",
		"1": "1",
		"2": "2",
		"3": "3",
		"4": "4",
		"5": "5",
		"6": "6",
		"7": "7",
		"8": "8",
		"9": "9",
		"0": "0",
		"Minus": "-",
		"Equal": "=",
		"Backspace": DEL_LEFT,
		"Delete": DEL,
		"Tab": "\t",
		"BracketLeft": "[",
		"BracketRight": "]",
		"BackSlash": "\\",
		"Semicolon": ";",
		"Apostrophe": "'",
		"Enter": CRLF,
		"Comma": ",",
		"Period": ".",
		"Slash": "/",
		"Shift+QuoteLeft": "~",
		"Shift+1": "!",
		"Shift+2": "@",
		"Shift+3": "#",
		"Shift+4": "$",
		"Shift+5": "%",
		"Shift+6": "^",
		"Shift+7": "&",
		"Shift+8": "*",
		"Shift+9": "(",
		"Shift+0": ")",
		"Shift+Minus": "_",
		"Shift+Equal": "+",
		"Shift+Backspace": "\b",
		"Shift+Tab": "\t",
		"Shift+BracketLeft": "{",
		"Shift+BracketRight": "}",
		"Shift+BackSlash": "|",
		"Shift+Semicolon": ":",
		"Shift+Apostrophe": '"',
		"Shift+Enter": CRLF,
		"Shift+Comma": "<",
		"Shift+Period": ">",
		"Shift+Slash": "?",
		"Shift+AsciiTilde": "~",
		"Shift+Exclam": "!",
		"Shift+At": "@",
		"Shift+NumberSign": "#",
		"Shift+Dollar": "$",
		"Shift+Percent": "%",
		"Shift+AsciiCircum": "^",
		"Shift+Ampersand": "&",
		"Shift+Asterisk": "*",
		"Shift+ParenLeft": "(",
		"Shift+ParenRight": ")",
		"Shift+UnderScore": "_",
		"Shift+Plus": "+",
		"Shift+BraceLeft": "{",
		"Shift+BraceRight": "}",
		"Shift+Bar": "|",
		"Shift+Colon": ":",
		"Shift+QuoteDbl": '"',
		"Shift+Less": "<",
		"Shift+Greater": ">",
		"Shift+Question": "?",
		"A": "a",
		"B": "b",
		"C": "c",
		"D": "d",
		"E": "e",
		"F": "f",
		"G": "g",
		"H": "h",
		"I": "i",
		"J": "j",
		"K": "k",
		"L": "l",
		"M": "m",
		"N": "n",
		"O": "o",
		"P": "p",
		"Q": "q",
		"R": "r",
		"S": "s",
		"T": "t",
		"U": "u",
		"V": "v",
		"W": "w",
		"X": "x",
		"Y": "y",
		"Z": "z",
		"Space": " ",
		"Shift+A": "A",
		"Shift+B": "B",
		"Shift+C": "C",
		"Shift+D": "D",
		"Shift+E": "E",
		"Shift+F": "F",
		"Shift+G": "G",
		"Shift+H": "H",
		"Shift+I": "I",
		"Shift+J": "J",
		"Shift+K": "K",
		"Shift+L": "L",
		"Shift+M": "M",
		"Shift+N": "N",
		"Shift+O": "O",
		"Shift+P": "P",
		"Shift+Q": "Q",
		"Shift+R": "R",
		"Shift+S": "S",
		"Shift+T": "T",
		"Shift+U": "U",
		"Shift+V": "V",
		"Shift+W": "W",
		"Shift+X": "X",
		"Shift+Y": "Y",
		"Shift+Z": "Z",
		"Up": UP,
		"Down": DOWN,
		"Right": RIGHT,
		"Left": LEFT,
	}


## Initialize (executed automatically by ForthTermLocal.new())
##
func _init(_forth: AMCForth, screen_material: ShaderMaterial) -> void:
	super(_forth)  # will assign _forth to forth
	_init_keymaps()
	_init_handlers()
	_blank = BL_CHAR.to_ascii_buffer()[0]
	# shader setup
	_screen_ram = PackedInt32Array()
	_screen_ram.resize(SCREEN_WIDTH * SCREEN_HEIGHT)
	_screen_material = screen_material
	_screen_material.set_shader_parameter("cols", SCREEN_WIDTH)
	_screen_material.set_shader_parameter("rows", SCREEN_HEIGHT)
	_screen_material.set_shader_parameter("display_power", true)
	set_power(true)  # Turn on the display!
	_set_screen_contents()
	_go_home()
	_last_msec = Time.get_ticks_msec()
	_last_msec -= _last_msec % (CURSOR_PERIOD_MSEC / 2)  # round to half a cursor cycle
	# Sort the special characters list
	_sp_chars_keys = _sp_chars.keys()
	_sp_chars_keys.sort_custom(_key_sort_func)
	# now safe to receive output
	connect_forth_output()
	forth.ClientConnected()


# save/load terminal screen ram
func save_state(cfg: ConfigFile) -> void:
	cfg.set_value("Computer", "screen_image", _screen_ram)


# load terminal screen ram from config file
func load_state(cfg: ConfigFile) -> void:
	if cfg.has_section_key("Computer", "screen_image"):
		_screen_ram = cfg.get_value("Computer", "screen_image")


func set_power(state: bool) -> void:
	_screen_material.set_shader_parameter("display_power", state)


# custom function for sorting special characters by length
func _key_sort_func(x, y) -> bool:
	return x.length() > y.length()


# Receive local key events from the owning node
func handle_key_event(evt: InputEvent) -> void:
	var keycode: String = OS.get_keycode_string(
		evt.get_key_label_with_modifiers()
	)
	if keycode in _us_key_map and forth.IsReadyForInput() and evt.is_pressed():
		forth.TerminalIn(_us_key_map[keycode])


# Called from owner _process
func update_time() -> void:
	var msec = Time.get_ticks_msec()
	if (msec < _last_msec) or (msec - _last_msec > CURSOR_PERIOD_MSEC / 2):
		# step _last_msec back to nearest half period
		_last_msec += (CURSOR_PERIOD_MSEC / 2)
		_cursor_state = not _cursor_state
		_screen_material.set_shader_parameter("cursor_state", _cursor_state)
		if _cursor_state:
			_blink_state = not _blink_state
			_screen_material.set_shader_parameter("blink_state", _blink_state)


# The forth output handler should be overridden in child classes
func _on_forth_output(_text: String) -> void:
	var text = _text
	while text.length():
		# look for a special character(s)
		var sp_found: bool = false
		for sch in _sp_chars_keys:  # look for longest special chars first!
			if text.find(sch) == 0:
				# do the thing
				text = _sp_chars[sch].call(text)
				sp_found = true
				break
		# if no special character, display one character
		if not sp_found:
			# display, but don't update screen yet
			_char_at_cursor(text[0].to_ascii_buffer()[0], false)
			_advance_cursor()
			text = text.substr(1)
	_set_screen_contents()


# Transfer cursor position to the shader
func _set_screen_cursor() -> void:
	_screen_material.set_shader_parameter("cursor_position", _cursor)


# Transfer screen buffer to the shader
func _set_screen_contents() -> void:
	_screen_material.set_shader_parameter("ram", _screen_ram)


# Display character at cursor, moving cursor
func _char_at_cursor(ch: int, set_contents: bool = true) -> void:
	_screen_ram[(_cursor.x - 1) + (_cursor.y - 1) * SCREEN_WIDTH] = ch + _mode
	if set_contents:
		_set_screen_contents()


# Advance cursor
func _advance_cursor() -> void:
	_cursor.x += 1
	if _cursor.x > SCREEN_WIDTH:
		_cursor.x = 1
		_cursor.y += 1
		if _cursor.y > SCREEN_HEIGHT:
			_cursor.y = SCREEN_HEIGHT
			_line_feed()
	_set_screen_cursor()


func _line_feed() -> void:
	_screen_ram = _screen_ram.slice(SCREEN_WIDTH)
	_screen_ram.append_array(blank_line)
	_set_screen_contents()
	_set_screen_cursor()


# Set cursor to home
func _go_home() -> void:
	_cursor.x = 1
	_cursor.y = 1
	_set_screen_cursor()


func _move_left() -> void:
	if _cursor.x > 1:
		_cursor.x -= 1
		_set_screen_cursor()


# ESCAPE CODE HANDLERS


func _do_atxy_start(_text: String) -> String:
	var h_pos := _text.find("H")
	if h_pos != -1:
		var ret := _text.substr(h_pos + 1)  # get everything past H
		var text := _text.substr(ATXY_START.length())
		var semi_pos := text.find(";")
		var col := text.substr(0, semi_pos).to_int()
		var row := text.substr(semi_pos + 1).to_int()
		_cursor.x = col
		_cursor.y = row
		_set_screen_cursor()
		return ret
	# not actually ATXY, return whatever's left
	return _text.substr(ATXY_START.length())


func _do_bsp(text: String) -> String:  # is this different from left cursor?
	if _cursor.x > 1:
		_cursor.x -= 1
		_set_screen_cursor()
	# remove the special character(s)
	return text.substr(BSP.length())


func _do_cr(text: String) -> String:
	_cursor.x = 1
	_set_screen_cursor()
	# remove the special character(s)
	return text.substr(CR_CHAR.length())


func _do_lf(text: String) -> String:
	if _cursor.y < SCREEN_HEIGHT:
		_cursor.y += 1
		_set_screen_cursor()
	else:
		_line_feed()
	# remove the special character(s)
	return text.substr(LF.length())


func _do_esc(text: String) -> String:
	_char_at_cursor(1)  # display a diamond
	_advance_cursor()
	# remove the special character(s)
	return text.substr(ESC.length())


func _do_del_left(text: String) -> String:
	_move_left()
	_char_at_cursor(0)
	# remove the special character(s)
	return text.substr(DEL_LEFT.length())


func _do_del(text: String) -> String:
	_char_at_cursor(0)
	# remove the special character(s)
	return text.substr(DEL.length())


func _do_up(text: String) -> String:
	if _cursor.y > 1:
		_cursor.y -= 1
		_set_screen_cursor()
	# remove the special character(s)
	return text.substr(UP.length())


func _do_down(text: String) -> String:
	if _cursor.y < SCREEN_HEIGHT:
		_cursor.y += 1
		_set_screen_cursor()
	# remove the special character(s)
	return text.substr(DOWN.length())


func _do_right(text: String) -> String:
	if _cursor.x < SCREEN_WIDTH:
		_cursor.x += 1
		_set_screen_cursor()
	# remove the special character(s)
	return text.substr(RIGHT.length())


func _do_left(text: String) -> String:
	_move_left()
	# remove the special character(s)
	return text.substr(LEFT.length())


func _do_clrline(text: String) -> String:
	var x = (_cursor.y - 1) * SCREEN_WIDTH
	for i in SCREEN_WIDTH:
		_screen_ram[x + i] = _blank
	_set_screen_contents()
	# remove the special character(s)
	return text.substr(CLRLINE.length())


func _do_clrscr(text: String) -> String:
	_screen_ram.fill(_blank)
	_go_home()
	_set_screen_contents()
	# remove the special character(s)
	return text.substr(CLRSCR.length())


func _do_pushxy(text: String) -> String:
	_save_cursor = _cursor
	_save_mode = _mode
	# remove the special character(s)
	return text.substr(PUSHXY.length())


func _do_popxy(text: String) -> String:
	_cursor = _save_cursor
	_mode = _save_mode
	_set_screen_cursor()
	# remove the special character(s)
	return text.substr(POPXY.length())


# Display modes


func _do_modesoff(text: String) -> String:
	_mode = 0
	# remove the special character(s)
	return text.substr(MODESOFF.length())


func _do_bold(text: String) -> String:
	_mode |= BOLD_MASK
	_mode &= ~LOWINTENSITY
	# remove the special character(s)
	return text.substr(BOLD.length())


func _do_lowint(text: String) -> String:
	_mode |= LOWINTENSITY
	_mode &= ~BOLD_MASK
	# remove the special character(s)
	return text.substr(LOWINT.length())


func _do_underline(text: String) -> String:
	_mode |= UNDERLINE_MASK
	# remove the special character(s)
	return text.substr(UNDERLINE.length())


func _do_blink(text: String) -> String:
	_mode |= BLINK_MASK
	# remove the special character(s)
	return text.substr(BLINK.length())


func _do_reverse(text: String) -> String:
	_mode |= REVERSE_MASK
	# remove the special character(s)
	return text.substr(REVERSE.length())


func _do_invisible(text: String) -> String:
	_mode |= INVISIBLE_MASK
	# remove the special character(s)
	return text.substr(INVISIBLE.length())
