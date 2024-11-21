class_name ForthImplementationBase
## Base class and utilities for Forth word definition collections
##

extends RefCounted

var forth: AMCForth


# Create with a reference to AMCForth
func _init(_forth: AMCForth):
	forth = _forth
	_scan_definitions()


# Scan source code for Forth word definitions
func _scan_definitions() -> void:
	var src = get_script().source_code
	var regex = RegEx.new()
	# Compile built-in WORD functions
	regex.compile("[^\"]##\\s+@WORD\\s+([^\\s]+)\\s*(IMMEDIATE)?.*\\n?\\r?func\\s+([^\\s(]+)")
	var res = regex.search_all(src)
	for item in res:
		forth.built_in_names.append(
			[item.strings[1], Callable(self, item.strings[3])]
		)
		if item.strings[2] == "IMMEDIATE":
			forth.immediate_names.append(item.strings[1])
	# Compile built-in WORDX run-time execution functions
	regex.compile("[^\"]##\\s+@WORDX\\s+([^\\s]+).*\\n?\\r?func\\s+([^\\s(]+)")
	res = regex.search_all(src)
	for item in res:
		forth.built_in_exec_functions.append(
			[item.strings[1], Callable(self, item.strings[2])]
		)