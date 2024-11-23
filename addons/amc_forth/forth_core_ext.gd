class_name ForthCoreExt
## @WORDSET Core Extended
##

extends ForthImplementationBase


## Initialize (executed automatically by ForthCoreExt.new())
##
## (1) All functions with "## @WORD <word>" comment will become
## the default implementation for the built-in word.
## (2) All functions with "## @WORDX <word>" comment will become
## the *compiled* implementation for the built-in word.
## (3) Define an IMMEDIATE function with "## @WORD <word> IMMEDIATE"
## (4) UP TO four comments beginning with "##" before function
## (5) Final comment must be "## @STACK" followed by stack def.
func _init(_forth: AMCForth) -> void:
	super(_forth)


## @WORD .(
## Begin parsing a comment, terminated by ')'. Comment text
## will emit to the terminal when it is compiled.
## @STACK ( - )
func dot_left_parenthesis() -> void:
	forth.core.start_parenthesis()
	forth.type()


## @WORD \
## Begin parsing a comment, terminated by end of line.
## @STACK ( - )
func back_slash() -> void:
	forth.push(ForthTerminal.CR.to_ascii_buffer()[0])
	parse()
	forth.two_drop()


## @WORD BUFFER:
## Create a dictionary entry for <name>, associated with n bytes of space.
## Usage: <n> BUFFER: <name>
## Executing <name> will return address of the starting byte on the stack.
## @STACK ( "name" n - )
func buffer_colon() -> void:
	forth.core.create()
	forth.core.allot()


## @WORD HEX
## Sets BASE to 16.
## @STACK ( - )
func decimal() -> void:
	forth.push(16)
	forth.core.base()
	forth.core.store()


## @WORD FALSE
## Return a false value: a single-cell with all bits clear.
## @STACK ( - flag )
func f_false() -> void:
	forth.push(forth.FALSE)


## @WORD MARKER
## Create a dictionary definition for <name>, to be used as a deletion
## boundary. When <name> is executed, remove the definition for <name>
## and all subsequent definitions. Usage: MARKER <name>
## @STACK ( "name" - )
func marker() -> void:
	if forth.create_dict_entry_name():
		# copy the execution token
		forth.ram.set_word(
			forth.dict_top, forth.address_from_built_in_function[marker_exec]
		)
		# store the dict_p value in the next cell
		forth.ram.set_word(forth.dict_top + ForthRAM.CELL_SIZE, forth.dict_p)
		forth.dict_top += ForthRAM.DCELL_SIZE
		# preserve the state
		forth.save_dict_top()


## @WORDX MARKER
func marker_exec() -> void:
	# execution time functionality of marker
	# set dict_p to the previous entry
	forth.dict_top = forth.ram.get_word(forth.dict_ip + ForthRAM.CELL_SIZE)
	forth.dict_p = forth.ram.get_word(forth.dict_top)
	forth.save_dict_top()
	forth.save_dict_p()


## @WORD NIP
## Drop second stack item, leaving top unchanged.
## @STACK ( x1 x2 - x2 )
func nip() -> void:
	forth.core.swap()
	forth.core.drop()


## @WORD PARSE
## Parse text to the first instance of char, returning the address
## and length of a temporary location containing the parsed text.
## Returns a counted string. Consumes the final delimiter.
## @STACK ( char - c_addr n )
func parse() -> void:
	var count: int = 0
	var ptr: int = forth.WORD_START + 1
	var delim: int = forth.pop()
	forth.core.source()
	var source_size: int = forth.pop()
	var source_start: int = forth.pop()
	forth.core.to_in()
	var ptraddr: int = forth.pop()
	forth.push(ptr)  # parsed text begins here
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
	forth.push(count)


## @WORD PICK
## Place a copy of the nth stack entry on top of the stack.
## The zeroth item is the top of the stack, so 0 pick is dup.
## @STACK ( +n - x )
func pick() -> void:
	var n = forth.pop()
	if n >= forth.data_stack.size():
		forth.util.rprint_term(" PICK outside data stack")
	else:
		forth.push(forth.data_stack[-n - 1])


## @WORD SOURCE-ID
## Return a value indicating current input source.
## Value is 0 for default user input, -1 for character string.
## @STACK ( - n )
func source_id() -> void:
	forth.push(forth.source_id)


## @WORD TO
## Store x in the data space associated with name (defined with VALUE).
## Usage: <x> TO <name>
## @STACK ( "name" x - )
func to() -> void:
	# get the name
	forth.push(ForthTerminal.BL.to_ascii_buffer()[0])
	forth.core.word()
	forth.core.count()
	var len: int = forth.pop()  # length
	var caddr: int = forth.pop()  # start
	var word: String = forth.util.str_from_addr_n(caddr, len)
	var token_addr_immediate = forth.find_in_dict(word)
	if not token_addr_immediate[0]:
		forth.util.print_unknown_word(word)
	else:
		# adjust to data field location
		forth.ram.set_word(
			token_addr_immediate[0] + ForthRAM.CELL_SIZE, forth.pop()
		)


## @WORD TRUE
## Return a true value, a single-cell value with all bits set.
## @STACK ( - flag )
func f_true() -> void:
	forth.push(forth.TRUE)


## @WORD TUCK
## Place a copy of the top stack item below the second stack item.
## @STACK ( x1 x2 - x2 x1 x2 )
func tuck() -> void:
	forth.core.swap()
	forth.push(forth.data_stack[forth.ds_p + 1])


## @WORD UNUSED
## Return u, the number of bytes remaining in the memory area
## where dictionary entries are constructed.
## @STACK ( - u )
func unused() -> void:
	forth.push(forth.DICT_TOP - forth.dict_top)


## @WORD VALUE
## Create a dictionary entry for name, associated with value x.
## Usage: <x> VALUE <name>
## @STACK ( "name" x - )
func value() -> void:
	var init_val: int = forth.pop()
	if forth.create_dict_entry_name():
		# copy the execution token
		forth.ram.set_word(
			forth.dict_top, forth.address_from_built_in_function[value_exec]
		)
		# store the initial value
		forth.ram.set_word(forth.dict_top + ForthRAM.CELL_SIZE, init_val)
		forth.dict_top += ForthRAM.DCELL_SIZE
		# preserve the state
		forth.save_dict_top()


## @WORDX VALUE
func value_exec() -> void:
	# execution time functionality of value
	# return contents of the cell after the execution token
	forth.push(forth.ram.get_word(forth.dict_ip + ForthRAM.CELL_SIZE))
