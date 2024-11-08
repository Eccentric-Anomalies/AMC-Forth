class_name ForthCoreExt
## Define built-in Forth words in the CORE EXTENSION word set
##

extends ForthImplementationBase


## Initialize (executed automatically by ForthCoreExt.new())
##
## (1) Append an array of <WORD>, <function> pairs to the Forth
## list of built-in words (built_in_names).
## (2) Append an array of <function> references to the Forth
## list of built-in execution-time functions (if any)
func _init(_forth: AMCForth) -> void:
	super(_forth)
	(
		forth
		. built_in_names
		. append_array(
			[
				[".(", dot_left_parenthesis],  # core ext
				["\\", back_slash],  # core ext
				["BUFFER:", buffer_colon],  # core ext
				["NIP", nip],  # core ext
				["PARSE", parse],  # core ext
				["PICK", pick],  # core ext
				["TO", to],  # core ext
				["TUCK", tuck],  # core ext
				["UNUSED", unused],  # core ext
				["VALUE", value],  # core ext
			]
		)
	)
	(
		forth
		. built_in_exec_functions
		. append_array(
			[
				value_exec,
			]
		)
	)


## .(
func dot_left_parenthesis() -> void:
	# Begin parsing a comment, terminated by ')'. Comment text
	# will emit to the terminal.
	# ( - )
	forth.core.start_parenthesis()
	forth.type()


## \
func back_slash() -> void:
	# Begin parsing a comment, terminated by end of line
	# ( - )
	forth.push_word(ForthTerminal.CR.to_ascii_buffer()[0])
	forth.parse()
	forth.two_drop()


## BUFFER:
func buffer_colon() -> void:
	# Create a dictionary entry for name associated with n bytes of space
	# n BUFFER: <name>
	# ( n - )
	# execution of <name> will return address of the starting byte ( - addr )
	forth.core.create()
	forth.core.allot()


## NIP
func nip() -> void:
	# drop second item, leaving top unchanged
	# ( x1 x2 - x2 )
	var t: int = forth.ram.get_int(forth.ds_p)
	forth.ds_p += ForthRAM.CELL_SIZE
	forth.ram.set_int(forth.ds_p, t)


## PARSE
func parse() -> void:
	# Parse text to the first instance of char, returning the address
	# and length of a temporary location containing the parsed text.
	# Returns an address with one byte available in front for forming
	# a character count. Consumes the final delimiter.
	# ( char - c_addr n )
	var count: int = 0
	var ptr: int = forth.WORD_START + 1
	var delim: int = forth.pop_word()
	forth.core.source()
	var source_size: int = forth.pop_word()
	var source_start: int = forth.pop_word()
	forth.core.to_in()
	var ptraddr: int = forth.pop_word()
	forth.push_word(ptr)  # parsed text begins here
	while true:
		var t: int = forth.ram.get_byte(
			source_start + forth.ram.get_word(ptraddr)
		)
		# increment the input pointer
		if t != 0:
			forth.ram.set_word(ptraddr, forth.ram.get_word(ptraddr) + 1)
		# a null character also stops the parse
		if t != 0 and t != delim:
			forth.ram.set_byte(ptr, t)
			ptr += 1
			count += 1
		else:
			break
	forth.push_word(count)


## PICK
func pick() -> void:
	# place a copy of the nth stack entry on top of the stack
	# zeroth item is the top of the stack so 0 pick is dup
	# ( +n - x )
	var t: int = forth.ram.get_int(forth.ds_p)
	forth.ram.set_int(
		forth.ds_p, forth.ram.get_int(forth.ds_p + (t + 1) * ForthRAM.CELL_SIZE)
	)


## TO
func to() -> void:
	# Store x in the data space associated with name (defined by value)
	# x TO <name> ( x - )
	# get the name
	forth.push_word(ForthTerminal.BL.to_ascii_buffer()[0])
	forth.core.word()
	forth.core.count()
	var len: int = forth.pop_word()  # length
	var caddr: int = forth.pop_word()  # start
	var word: String = forth.util.str_from_addr_n(caddr, len)
	var token_addr = forth.find_in_dict(word)
	if not token_addr:
		forth.util.print_unknown_word(word)
	else:
		# adjust to data field location
		token_addr += ForthRAM.CELL_SIZE
		forth.ram.set_word(token_addr, forth.pop_word())


## TUCK
func tuck() -> void:
	# place a copy of the top stack item below the second stack item
	# ( x1 x2 - x2 x1 x2 )
	forth.ram.set_int(
		forth.ds_p - ForthRAM.CELL_SIZE, forth.ram.get_int(forth.ds_p)
	)
	forth.ram.set_int(
		forth.ds_p, forth.ram.get_int(forth.ds_p + ForthRAM.CELL_SIZE)
	)
	forth.ram.set_int(
		forth.ds_p + ForthRAM.CELL_SIZE,
		forth.ram.get_int(forth.ds_p - ForthRAM.CELL_SIZE)
	)
	forth.ds_p -= ForthRAM.CELL_SIZE


## UNUSED
func unused() -> void:
	# Return u, the number of bytes remaining in the memory area
	# where dictionary entries are constructed.
	# ( - u )
	forth.push_word(forth.DICT_TOP - forth.dict_top)


## VALUE
func value() -> void:
	# Create a dictionary entry for name, associated with value x.
	# ( x - )
	forth.create_dict_entry_name()
	# copy the execution token
	forth.ram.set_word(
		forth.dict_top, forth.address_from_built_in_function[value_exec]
	)
	# store the initial value
	forth.ram.set_word(forth.dict_top + ForthRAM.CELL_SIZE, forth.pop_word())
	forth.dict_top += ForthRAM.DCELL_SIZE


## VALUE run-time implementation
func value_exec() -> void:
	# execution time functionality of _value
	# return contents of the cell after the execution token
	forth.push_word(forth.ram.get_word(forth.dict_ip + ForthRAM.CELL_SIZE))