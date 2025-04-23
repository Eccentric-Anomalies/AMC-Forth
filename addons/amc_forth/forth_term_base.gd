class_name ForthTermBase
## Base class and utilities for Forth terminals
##

extends RefCounted

const SCREEN_WIDTH := 80
const SCREEN_HEIGHT := 24

const BSP = "\u0008"
const CR_CHAR = "\u000D"
const LF = "\u000A"
const CRLF = "\r\n"
const ESC = "\u001B"
const DEL_LEFT = "\u007F"
const BL_CHAR = " "
const DEL = "\u001B[3~"
const UP = "\u001B[A"
const DOWN = "\u001B[B"
const RIGHT = "\u001B[C"
const LEFT = "\u001B[D"
const CLRLINE = "\u001B[2K"
const CLRSCR = "\u001B[2J"
const PUSHXY = "\u001B7"
const POPXY = "\u001B8"
const MODESOFF = "\u001B[m"
const BOLD = "\u001B[1m"
const LOWINT = "\u001B[2m"
const UNDERLINE = "\u001B[4m"
const BLINK = "\u001B[5m"
const REVERSE = "\u001B[7m"
const INVISIBLE = "\u001B[8m"
const CURSORHIDE = "\u001B[?25l"
const CURSORSHOW = "\u001B[?25h"
const ATXY_START = "\u001B["

var forth: AMCForth
var blank_line: PackedInt32Array
var screen_width: int
var screen_height: int


# Create with a reference to AMCForth
# Option to override the default WIDTH (80) and HEIGHT (24) of the terminal screen
func _init(_forth: AMCForth, width: int = SCREEN_WIDTH, height: int = SCREEN_HEIGHT):
	forth = _forth
	screen_width = width
	screen_height = height
	# create a blank line
	blank_line = PackedInt32Array()
	blank_line.resize(screen_width)
	blank_line.fill(" ".to_ascii_buffer()[0])


# Connect to the Forth output
func connect_forth_output() -> void:
	forth.TerminalOut.connect(_on_forth_output)


# The forth output handler should be overridden in child classes
func _on_forth_output(_text: String) -> void:
	pass
