using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AMCForth : Godot.RefCounted
{
	[Signal]
	public delegate void TerminalOutEventHandler(string text);
	[Signal]
	public delegate void TerminalInReadyEventHandler();


	// control flow address types
	public enum CFType {ORIG, DEST}

	public const string Banner = "AMC Forth";
	public const string ConfigFileName = "user://ForthState.cfg";


	// Memory Map
	public const int RamSize = 0x10000;
	// BYTES
	// Dictionary
	public const int DictStart = 0x0100;
	// BYTES
	public const int DictSize = 0x08000;
	public const int DictTop = DictStart + DictSize;

	// Dictionary scratch space
	public const int DictBuffSize = 0x040;
	// word-sized
	public const int DictBuffStart = DictTop;
	public const int DictBuffTop = DictBuffStart + DictBuffSize;

	// Input Buffer
	public const int BuffSourceSize = 0x0100;
	// bytes
	public const int BuffSourceStart = DictBuffTop;
	public const int BuffSourceTop = BuffSourceStart + BuffSourceSize;

	// File Buffers
	public const int FileBuffQty = 8;
	// number of simultaneous open files possible
	public const int FileBuffIdOffset = 0;
	// offset in buffer to fileid
	public const int FileBuffPtrOffset = ForthRAM.CellSize;
	// location of pointer
	public const int FileBuffDataOffset = ForthRAM.CellSize * 2;
	// location of buff data
	public const int FileBuffSize = 0x0100;
	// bytes, overall
	public const int FileBuffDataSize = FileBuffSize - FileBuffDataOffset;
	public const int FileBuffStart = BuffSourceTop;
	public const int FileBuffTop = FileBuffStart + FileBuffSize * FileBuffQty;

	// Pointer to the parse position in the TERMINAL buffer
	public const int BuffToIn = FileBuffTop;
	public const int BuffToInTop = BuffToIn + ForthRAM.CellSize;

	// Temporary word storage (used by WORD)
	public const int WordBuffSize = 0x0100;
	public const int WordBuffStart = BuffToInTop;
	public const int WordBuffTop = WordBuffStart + WordBuffSize;

	// BASE cell
	public const int Base = WordBuffTop;

	// DICT_TOP_PTR cell
	public const int DictTopPtr = Base + ForthRAM.CellSize;

	// DICT_PTR
	public const int DictPtr = DictTopPtr + ForthRAM.CellSize;


	// IO SPACE - cell-sized ports identified by port # ranging from 0 to 255
	public const int IoOutPortQty = 0x0100;
	public const int IoOutTop = RamSize;
	public const int IoOutStart = IoOutTop - IoOutPortQty * ForthRAM.CellSize;
	public const int IoInPortQty = 0x0100;
	public const int IoInTop = IoOutStart;
	public const int IoInStart = IoInTop - IoInPortQty * ForthRAM.CellSize;
	public const int IoInMapTop = IoInStart;

	// xt for every port that is being listened on
	public const int IoInMapStart = IoInMapTop - IoInPortQty * ForthRAM.CellSize;

	// PERIODIC TIMER SPACE
	public const int PeriodicTimerQty = 0x080;
	// Timer IDs 0-127, stored as @addr: msec, xt
	public const int PeriodicTop = IoInStart;


	public const int PeriodicStart = (PeriodicTop - PeriodicTimerQty * ForthRAM.CellSize * 2);


	// Add more pointers here
	public const int True = - 1;
	public const int False = 0;

	// Size of terminal command history
	public const int MaxBufferSize = 20;

	public const int DataStackSize = 100;
	public const int DataStackTop = DataStackSize - 1;

	public const int ReturnStackSize = 100;

	// Masks for built-in execution tokens
	public const uint XtUnusedMask = 0x80000000;
	public const uint BuiltInXtMask = 0x040000000;
	public const uint BuiltInXtXMask = 0x020000000;

	// Ensure we don't generate tokens that are larger than the CELL_SIZE


	public const uint BuiltInMask =  ~ (XtUnusedMask | BuiltInXtMask | BuiltInXtXMask);
	
// Smudge bit mask
	public const int SmudgeBitMask = 0x80;

	// Immediate bit mask
	public const int ImmediateBitMask = 0x40;

	// Largest name length
	public const int MaxNameLength = 0x3f;


	// Reference to the physical memory and utilities
	public ForthRAM Ram;
	public ForthUtil Util;


	// Forth Word Classes
	public Forth.AMCExt.AMCExtSet AMCExtWords;
	public Forth.String.StringSet StringWords;
	public Forth.CommonUse.CommonUseSet CommonUseWords;
	public Forth.Core.CoreSet CoreWords;
	public Forth.CoreExt.CoreExtSet CoreExtWords;
	public Forth.Double.DoubleSet DoubleWords;

	// Forth built-in meta-data  # FIXME
	public Dictionary WordDescription = new Dictionary{};
	public Dictionary WordStackdef = new Dictionary{};
	public Dictionary WordWordset = new Dictionary{};
	public Dictionary WordsetWords = new Dictionary{};


	// The Forth data stack pointer is in byte units
	// The Forth dictionary space
	public int DictP;
	// position of last link
	public int DictTopP;
	// position of next new link to create
	public int DictIp = 0;

	// code field pointer set to current execution point
	// Forth compile state
	public bool State = false;


	// Forth source ID
	public int SourceId = 0;
	// 0 default, -1 ram buffer, else file id
	public Stack<int> SourceIdStack = new();

	// Built-In names have a run-time definition
	// These are "<WORD>", <run-time function> pairs that are defined by each
	// Forth implementation class (e.g. ForthDouble, etc.)
	// public Array BuiltInNames = new Array{}; FIXME

	// list of built-in functions that have different
	// compiled (execution token) behavior.
	// These are <run-time function> items that are defined by each
	// Forth implementation class (e.g. ForthDouble, etc.) when a
	// different *compiled* behavior is required
	// Each item is a [<name>, <callable>] pair
	// public Array BuiltInExecFunctions = new Array{}; FIXME

	// List of built-in names that are IMMEDIATE by default
	// public Array ImmediateNames = new Array{}; FIXME


	// get "address" from built-in function
	// public Dictionary AddressFromBuiltInFunction = new Dictionary{}; FIXME

	// get built-in function from "address"
	// public Dictionary BuiltInFunctionFromAddress = new Dictionary{}; FIXME

	// get built-in function from word
	// public Dictionary BuiltInFunction = new Dictionary{}; FIXME


	// Forth : exit flag (true if exit has been called)
	public bool ExitFlag = false;


	// Forth: data stack
	public int[] DataStack = new int[DataStackSize];
	public int DsP;


	// Forth: return stack
	public int[] ReturnStack = new int[ReturnStackSize];
	public int RsP;


	// Output handlers
	public System.Collections.Generic.Dictionary<int, Godot.StringName> OutputPortMap = new();

	// structure of an input port event (port id, data value)
	public readonly struct PortEvent
	{
		public PortEvent(int port, int value)
		{
			Port = port;
			Value = value;
		}

		public int Port { get; }
		public int Value { get;}

		public override string ToString() => $"({Port}, {Value})";
	}

	// Input event list of incoming PortEvent data
	public List<PortEvent> InputPortEvents = new();

	public readonly struct TimerStruct
	{
		public TimerStruct(int msec, int xt, Timer timer)
		{
			MSec = msec;
			Xt = xt;
			Timer = timer;
		}

		public int MSec { get; }
		public int Xt { get;}
		public Timer Timer { get; }

		public override string ToString() => $"({MSec}, {Xt}, {Timer})";
	}


	// Periodic timer list
	public System.Collections.Generic.Dictionary<int,TimerStruct> PeriodicTimerMap = new();

	// Timer events queue
	public Queue<int> TimerEvents = new();

	// Owning Node
	protected Godot.Node _Node;

	// State file
	protected Godot.ConfigFile _Config;

	protected bool _DataStackUnderflow = false;


	// terminal scratchpad and buffer
	protected string _TerminalPad = "";
	protected int _PadPosition = 0;
	protected int _ParsePointer = 0;
	protected List<string> _TerminalBuffer = new();
	protected int _BufferIndex = 0;


	// Forth : execution dict_ip stack
	protected Stack<int> _DictIpStack = new();

	public readonly struct ControlFlowItem
	{
		public ControlFlowItem(CFType type, int addr)
		{
			AddrType = type;
			Addr = addr;
		}

		public CFType AddrType { get; }
		public int Addr { get;}

		public override string ToString() => $"({AddrType}, {Addr})";
	}

	// Forth: control flow stack. Entries are in the form
	// [orig | dest, address]
	protected Stack<ControlFlowItem> _ControlFlowStack = new();


	// Forth: loop control flow stack for LEAVE ORIG entries only!
	protected Stack<int> _LeaveControlFlowStack = new();


	// Thread data
	protected System.Threading.Thread _Thread;
	protected Godot.Semaphore _InputReady;
	protected bool _OutputDone;


	// Client connect count
	protected int _ClientConnections = 0;


	// File access
	// map Forth fileid to FileAccess objects
	// file_id is the address of the file's buffer structure
	// the first cell in the structure is the file access mode bits
	//protected Dictionary _FileIdDict = new Dictionary{};
	protected System.Collections.Generic.Dictionary<int, Godot.FileAccess> _FileIdDict = new();


	// allocate a buffer for the provided file handle and mode
	// return the file id or zero if none available
	public int AssignFileId(Godot.FileAccess file, int new_mode)
	{
		for (int i = 0; i < FileBuffQty; i++)
		{
			var addr = i * FileBuffSize + FileBuffStart;
			var mode = Ram.GetInt(addr + FileBuffIdOffset);
			if(mode == 0)
			{

				// available file handle
				Ram.SetInt(addr + FileBuffIdOffset, new_mode);
				Ram.SetInt(addr + FileBuffPtrOffset, 0);
				_FileIdDict[addr] = file;
				return addr;
			}
			addr += FileBuffSize;
		}
		return 0;
	}


	public Godot.FileAccess GetFileFromId(int id)
	{
		if (_FileIdDict.ContainsKey(id)) 
		{
			return _FileIdDict[id];
		}
		else
		{
			return null;
		}
	}


// releases an file buffer, and closes the associated file, if open
	public void FreeFileId(int id)
	{
		var file = _FileIdDict[id];
		if(file.IsOpen())
		{
			file.Close();
		}

	// clear the buffer entry
		Ram.SetInt(id + FileBuffIdOffset, 0);

		// erase the dictionary entry
		_FileIdDict.Remove(id);
	}


	public void ClientConnected()
	{
		if(_ClientConnections == 0)
		{
			EmitSignal("TerminalOut", GetBanner() + Forth.Terminal.CRLF);
			_ClientConnections += 1;
		}
	}


	public void CloseAllFiles()
	{
		foreach(int id in _FileIdDict.Keys)
		{
			FreeFileId(id);
		}
	}


// pause until Forth is ready to accept inupt
	public bool IsReadyForInput()
	{
		return _OutputDone;
	}


// preserve Forth memory and state
	public void SaveSnapshot()
	{
		_Config.Clear();
		CloseAllFiles();
		Ram.SaveState(_Config);
		_Config.Save(ConfigFileName);
	}


// restore Forth memory and state
	public void LoadSnapshot()
	{

		// stop all periodic timers
		RemoveAllTimers();

		// if a timer comes in, it should see nothing to do
		_Config.Load(ConfigFileName);
		Ram.LoadState(_Config);

		// restore shadowed registers
		RestoreDictP();
		RestoreDictTop();

		// start all configured periodic timers
		RestoreAllTimers();
	}


// handle editing input strings in interactive mode
	public void TerminalIn(string text)
	{
		var in_str = text;
		var echo_text = "";
		var buffer_size = _TerminalBuffer.Count;
		while(in_str.Length > 0)
		{
			if(in_str.Find(Forth.Terminal.DEL_LEFT) == 0)
			{
				_PadPosition = Mathf.Max(0, _PadPosition - 1);
				if(_TerminalPad.Length != 0)
				{

					// shrink if deleting from end, else replace with space
					if(_PadPosition == _TerminalPad.Length - 1)
					{
						_TerminalPad = _TerminalPad.Left(_PadPosition);
					}
					else
					{
						_TerminalPad = _TerminalPad.Left(_PadPosition) + " " + _TerminalPad.Substring(_PadPosition+1);
					}
				}

		// reconstruct the changed entry, with correct cursor position
				echo_text = _RefreshEditText();
				in_str = in_str.Substring(Forth.Terminal.DEL_LEFT.Length);
			}
			else if(in_str.Find(Forth.Terminal.DEL) == 0)
			{

				// do nothing unless cursor is in text
				if(_PadPosition <= _TerminalPad.Length)
				{
					_TerminalPad = _TerminalPad.Left(_PadPosition) + _TerminalPad.Substring(_PadPosition+1);
				}

			// reconstruct the changed entry, with correct cursor position
				echo_text = _RefreshEditText();
				in_str = in_str.Substring(Forth.Terminal.DEL.Length);
			}
			else if(in_str.Find(Forth.Terminal.LEFT) == 0)
			{
				_PadPosition = Mathf.Max(0, _PadPosition - 1);
				echo_text = Forth.Terminal.LEFT;
				in_str = in_str.Substring(Forth.Terminal.LEFT.Length);
			}
			else if(in_str.Find(Forth.Terminal.RIGHT) == 0)
			{
				_PadPosition += 1;
				if(_PadPosition > _TerminalPad.Length)
				{
					_PadPosition = _TerminalPad.Length;
				}
				else
				{
					echo_text = Forth.Terminal.RIGHT;
				}
				in_str = in_str.Substring(Forth.Terminal.RIGHT.Length);
			}
			else if(in_str.Find(Forth.Terminal.UP) == 0)
			{
				if(buffer_size != 0)
				{
					_BufferIndex = Mathf.Max(0, _BufferIndex - 1);
					echo_text = _SelectBufferedCommand();
				}
				in_str = in_str.Substring(Forth.Terminal.UP.Length);
			}
			else if(in_str.Find(Forth.Terminal.DOWN) == 0)
			{
				if(buffer_size != 0)
				{
					_BufferIndex = Mathf.Min(_TerminalBuffer.Count - 1, _BufferIndex + 1);
					echo_text = _SelectBufferedCommand();
				}
				in_str = in_str.Substring(Forth.Terminal.DOWN.Length);
			}
			else if(in_str.Find(Forth.Terminal.LF) == 0)
			{
				echo_text = "";
				in_str = in_str.Substring(Forth.Terminal.LF.Length);
			}
			else if(in_str.Find(Forth.Terminal.CR) == 0)
			{
				// only add to the buffer if it's different from the top entry
				// and not blank!
				if((_TerminalPad.Length != 0) && ((buffer_size == 0) || (_TerminalBuffer[_TerminalBuffer.Count - 1] != _TerminalPad)))
				{
					_TerminalBuffer.Add(_TerminalPad);
					// if we just grew too big...
					if(buffer_size == MaxBufferSize)
					{
						_TerminalBuffer.RemoveAt(0);
					}
				}
				_BufferIndex = _TerminalBuffer.Count;
				// refresh the line in the terminal
				_PadPosition = _TerminalPad.Length;
				EmitSignal("TerminalOut", _RefreshEditText());
				// text is ready for the Forth interpreter
				_InputReady.Post();
				in_str = in_str.Substring(Forth.Terminal.CR.Length);
			}
			// not a control character(s)
			else {
				echo_text = in_str.Left(1);
				in_str = in_str.Substring(1);
				if(_PadPosition < _TerminalPad.Length)
				{
					_TerminalPad = _TerminalPad.Left(_PadPosition) + echo_text + _TerminalPad.Substring(_PadPosition+1);				}
				else
				{
					_TerminalPad += echo_text;
				}
				_PadPosition += 1;
			}
			EmitSignal("TerminalOut", echo_text);
		}
	}

	public readonly struct DictResult
	{
		public DictResult(int addr, bool isImmediate)
		{
			Addr = addr;
			IsImmediate = isImmediate;
		}

		public int Addr { get; }
		public bool IsImmediate { get;}

		public override string ToString() => $"({Addr}, {IsImmediate})";
	}

// Find word in dictionary, starting at address of top
// Returns a list consisting of:
//  > the address of the first code field (zero if not found)
//  > a boolean true if the word is defined as IMMEDIATE
	public DictResult FindInDict(string word)
	{
		if(DictP == DictTopP)
		{
			// dictionary is empty
			return new(0, false);
		}
		// stuff the search string in data memory
		Util.CstringFromStr(DictBuffStart, word);
		// make a temporary pointer
		var p = DictP;
		while(p !=  - 1) // <empty>
		{
			Push(DictBuffStart);	// c-addr
			CoreWords.Count.Call();	// search word in addr  # addr n
			Push(p + ForthRAM.CellSize);	// entry name  # addr n c-addr
			CoreWords.Count.Call();	// candidate word in addr			# addr n addr n
			var n_raw_length = Pop();	// addr n addr
			var n_length = n_raw_length & ~ (SmudgeBitMask | ImmediateBitMask);
			Push(n_length);	// strip the SMUDGE and IMMEDIATE bits and restore # addr n addr n
			// only check if the entry has a clear smudge bit
			if((n_raw_length & SmudgeBitMask) == 0)
			{
				StringWords.Compare.Call();
				// is this the correct entry?
				if(Pop() == 0)
				{
					// found it. Link address + link size + string length byte + string, aligned
					Push(p + ForthRAM.CellSize + 1 + n_length);	// n
					CoreWords.Aligned.Call(); // a
					return new(Pop(), (n_raw_length & ImmediateBitMask) != 0);
				}
			}
			else
			{
				// clean up the stack
				PopDword(); // addr n
				PopDword();
			}
			// not found, drill down to the next entry
			p = Ram.GetInt(p);
		}
		// exhausted the dictionary, finding nothing
		return new(0, false);	
	}


// Internal utility function for creating the start of
// a dictionary entry. The next thing to follow will be
// the execution token. Upon exit, dict_top will point to the
// aligned position of the execution token to be.
// Accepts an optional smudge state (default false).
// Returns the address of the name length byte or zero on fail.
	public int CreateDictEntryName(bool smudge = false)
	{
		// ( - )
		// Grab the name
		CoreExtWords.ParseName.Call();
		var len = Pop();		// length
		var caddr = Pop();		// start
		if(len <= MaxNameLength)
		{
			// poke address of last link at next spot, but only if this isn't
			// the very first spot in the dictionary
			if(DictTopP != DictP)
			{
				// align the top pointer, so link will be word-aligned
				CoreWords.Align.Call();
				Ram.SetInt(DictTopP, DictP);
			}
			// move the top link
			DictP = DictTopP;
			SaveDictP();
			DictTopP += ForthRAM.CellSize;
			// poke the name length, with a smudge bit if needed
			var smudge_bit = ( smudge ? SmudgeBitMask : 0 );
			Ram.SetByte(DictTopP, len | smudge_bit);
			// preserve the address of the length byte
			var ret = DictTopP;
			DictTopP += 1;
			// copy the name
			Push(caddr);
			Push(DictTopP);
			Push(len);
			CoreWords.Move.Call();
			DictTopP += len;
			CoreWords.Align.Call();			// will save dict_top
			// the address of the name length byte
			return ret;
		}
		return 0;
	}


// Unwind pointers and stacks to reverse the effect of any
// colon definition currently underway.
	public void UnwindCompile()
	{
		if(State)
		{
			State = false;
			// reset the control flow stack
			CfReset();
			// restore the original dictionary state
			DictTopP = DictP;
			DictP = Ram.GetInt(DictP);
		}
	}

// Forth Input and Output Interface

// Register an output signal handler (port triggers message out)
// Message will fire with Forth OUT ( x p - )
	public void AddOutputSignal(int port, Signal s)
	{
		OutputPortMap[port] = s.Name;
	}

// Get a reference to a input port handler function.
// In Gdscript use <signal>.Connect(<return value>)
	public Action<int> GetInputReceiver(int port)
	{
		return (int value) => InsertNewEvent(port, value);
	}


// Utility function to add an input event to the queue
	protected void InsertNewEvent(int port, int value)
	{
		var item = new PortEvent(port, value);
		if(!InputPortEvents.Contains(item))
		{
			InputPortEvents.Add(item);
			// bump the semaphore count
			_InputReady.Post();
		}
	}



// Start a periodic timer with id to call an execution token
// This is only called from within Forth code!
	public void StartPeriodicTimer(int id, int msec, int xt)
	{
        void signalReceiver() => HandleTimeout(id);

        // save info
        var timer = new Timer();
		PeriodicTimerMap[id] = new(msec, xt, timer);
		timer.WaitTime = msec / 1000.0;
		timer.Autostart = true;
		timer.Timeout += signalReceiver;
		_Node.CallDeferred("add_child", timer);
	}


// Utility function to service periodic timer expirations
	protected void HandleTimeout(int id)
	{
		if(!TimerEvents.Contains(id))		// don't allow timer events to stack..
		{
			TimerEvents.Enqueue(id);
			// bump the semaphore count
			_InputReady.Post();
		}
	}


// Stop a timer without erasing the map entry
	protected void StopTimer(int id)
	{
		var timer = PeriodicTimerMap[id].Timer;
		timer.Stop();
		_Node.RemoveChild(timer);
	}


// Stop a single timer
	protected void RemoveTimer(int id)
	{
		if(PeriodicTimerMap.ContainsKey(id))
		{
			StopTimer(id);
			PeriodicTimerMap.Remove(id);
		}
	}


// Stop all timers
	protected void RemoveAllTimers()
	{
		foreach(int id in PeriodicTimerMap.Keys)
		{
			StopTimer(id);
		}
		PeriodicTimerMap.Clear();
	}


// Create and start all configured timers
	protected void RestoreAllTimers()
	{
		for (int id = 0; id < PeriodicTimerQty; id++)
		{
			var addr = PeriodicStart + ForthRAM.CellSize * 2 * id;
			var msec = Ram.GetInt(addr);
			var xt = Ram.GetInt(addr + ForthRAM.CellSize);
			if(xt != 0)
			{
				StartPeriodicTimer(id, msec, xt);
			}
		}
	}


// Forth Data Stack Push and Pop Routines

	public void Push(int val)
	{
		DsP -= 1;
		DataStack[DsP] = val;
	}


	public int Pop()
	{
		if(DsP < DataStackSize)
		{
			DsP += 1;
			return DataStack[DsP - 1];
		}
		Util.RprintTerm(" Data stack underflow");
		return 0;
	}


	public void PushDint(long val)
	{
		var t = ForthRAM.Split64(val);
		Push((int)Convert.ToUInt32(t.Lo));
		Push((int)Convert.ToUInt32(t.Hi));
	}


	public long PopDint()
	{
		return ForthRAM.Combine64(Pop(), Pop());
	}

// Forth Return Stack Push and Pop Routines

	public void RPush(int val)
	{
		RsP -= 1;
		ReturnStack[RsP] = val;
	}


	public int RPop()
	{
		if(RsP < ReturnStackSize)
		{
			RsP += 1;
			return ReturnStack[RsP - 1];
		}
		Util.RprintTerm(" Return stack underflow");
		return 0;
	}


	public void RPushDint(long val)
	{
		var t = ForthRAM.Split64(val);
		RPush((int)Convert.ToUInt32(t.Lo));
		RPush((int)Convert.ToUInt32(t.Hi));
	}


	public long RPopDint()
	{
		return ForthRAM.Combine64(RPop(), RPop());
	}


// top of stack is 0, next dint is at 2, etc.
	public long GetDint(int index)
	{
		return ForthRAM.Combine64(DataStack[DsP + index], DataStack[DsP + index + 1]);
	}


	public void SetDint(int index, long value)
	{
		var s = ForthRAM.Split64(value);
		DataStack[DsP + index] = (int)Convert.ToUInt32(s.Hi);
		DataStack[DsP + index + 1] = (int)Convert.ToUInt32(s.Lo);
	}


	public void PushDword(ulong value)
	{
		var s = ForthRAM.Split64((int)value);
		Push((int)Convert.ToUInt32(s.Lo));
		Push((int)Convert.ToUInt32(s.Hi));
	}


	public void SetDword(int index, ulong value)
	{
		var s = ForthRAM.Split64((int)value);
		DataStack[DsP + index] = (int)Convert.ToUInt32(s.Hi);
		DataStack[DsP + index + 1] = (int)Convert.ToUInt32(s.Lo);
	}


	public long PopDword()
	{
		return ForthRAM.Combine64(Pop(), Pop());
	}


// top of stack is -1, next dint is at -3, etc.
	public ulong GetDword(int index)
	{
		return (ulong) ForthRAM.Combine64(DataStack[DsP + index], DataStack[DsP + index + 1]);
	}


// save the internal top of dict pointer to RAM
	public void SaveDictTop()
	{
		Ram.SetInt(DictTopPtr, DictTopP);
	}


// save the internal dict pointer to RAM
	public void SaveDictP()
	{
		Ram.SetInt(DictPtr, DictP);
	}


// retrieve the internal top of dict pointer from RAM
	public void RestoreDictTop()
	{
		DictTopP = Ram.GetInt(DictTopPtr);
	}


// retrieve the internal dict pointer from RAM
	public void RestoreDictP()
	{
		DictP = Ram.GetInt(DictPtr);
	}
	
// dictionary instruction pointer manipulation
// push the current dict_ip
	public void PushIp()
	{
		_DictIpStack.Push(DictIp);
	}


	public void PopIp()
	{
		DictIp = _DictIpStack.Pop();
	}


	public bool IpStackIsEmpty()
	{
		return _DictIpStack.Count == 0;
	}

// compiled word control flow stack

// reset the stack
	public void CfReset()
	{
		_ControlFlowStack = new();
	}


	public void LcfReset()
	{
		_LeaveControlFlowStack = new();
	}


	protected void _CfPush(ControlFlowItem item)
	{
		_ControlFlowStack.Push(item);
	}


	public void LcfPush(int item)
	{
		_LeaveControlFlowStack.Push(item);
	}


// push an ORIG word
	public void CfPushOrig(int addr)
	{
		_CfPush(new(CFType.ORIG, addr));
	}


// push an DEST word
	public void CfPushDest(int addr)
	{
		_CfPush(new(CFType.DEST, addr));
	}


// pop a word
	protected ControlFlowItem _CfPop()
    {
        if (!CfStackIsEmpty())
        {
            return _ControlFlowStack.Pop();
        }
        throw new InvalidOperationException("Unbalanced control structure detected.");
    }


    public int LcfPop()
	{
		return _LeaveControlFlowStack.Pop();
	}


// check for items in the leave control flow stack
	public bool LcfIsEmpty()
	{
		return _LeaveControlFlowStack.Count == 0;
	}


// check for ORIG at top of stack
	public bool CfIsOrig()
	{
		return _ControlFlowStack.Peek().AddrType == CFType.ORIG;
	}


// check for DEST at top of stack
	public bool CfIsDest()
	{
		return _ControlFlowStack.Peek().AddrType == CFType.DEST;
	}


// pop an ORIG word
	public int CfPopOrig()
	{
		if(CfIsOrig())
		{
			return _CfPop().Addr;
		}
        throw new InvalidOperationException("Control structure expected ORIG but sees DEST.");
	}


// pop an DEST word
	public int CfPopDest()
	{
		if(CfIsDest())
		{
			return _CfPop().Addr;
		}
        throw new InvalidOperationException("Control structure expected DEST but sees ORIG.");
	}


// control flow stack is empty
	public bool CfStackIsEmpty()
	{
		return _ControlFlowStack.Count == 0;
	}


// control flow stack PICK (implements CS-PICK)
	public void CfStackPick(int item)
	{
		_CfPush(_ControlFlowStack.ToArray()[item]);
	}


// control flow stack ROLL (implements CS-ROLL)
	public void CfStackRoll(int item)
	{
		Stack<ControlFlowItem> tempStack = new();
		ControlFlowItem temp;
		for (int i = 0; i < item; i++)
		{
			tempStack.Push(_CfPop());
		}
		temp = _CfPop();
		for (int i = 0; i < item; i++)
		{
			_CfPush(tempStack.Pop());
		}
		_CfPush(temp);
	}

// PRIVATES
// Called when AMCForth.new() is executed
// This will cascade instantiation of all the Forth implementation classes
// and initialize dictionaries for relating built-in words and addresses
	public void Initialize(Godot.Node node)
	{
		// save the instantiating node
		_Node = node;

		// seed the randomizer
		GD.Randomize();

		// create a config file
		_Config = new();
		// the top of the dictionary can't overlap the high-memory stuff
		System.Diagnostics.Debug.Assert(DictTop < PeriodicStart);
		Ram = new();
		Ram.Allocate(RamSize);
		Util = new();
		Util.Initialize(this);
		// Instantiate Forth word definitions
		CommonUseWords = new(this);
		CoreWords = new(this);
		CoreExtWords = new(this);
		DoubleWords = new(this);
		StringWords = new(this);
		AMCExtWords = new(this);

		// Initialize the data stack pointer
		DsP = DataStackSize;

		// Initialize the return stack pointer
		RsP = ReturnStackSize;

		// set the terminal link in the dictionary
		Ram.SetInt(DictP,  - 1);

		// reset the buffer pointer
		Ram.SetInt(BuffToIn, 0);

		// set the base
		CoreWords.Decimal.Call();

		// initialize dictionary pointers and save them to RAM
		// FIXME note these have to be initialized when re-loading state
		DictP = DictStart;
		// position of last link
		SaveDictP();
		DictTopP = DictStart;
		// position of next new link to create
		SaveDictTop();

		// Launch the AMC Forth thread
		_Thread = new System.Threading.Thread(() => InputThread());

		// end test
		_InputReady = new();
		_Thread.Start();
		_OutputDone = true;
		GD.Print(GetBanner());
	}


// AMC Forth name with version
	protected static string GetBanner()
	{
		return Banner + " " + "Ver. " + ForthVersion.Ver;
	}


	protected void InputThread()
	{
		while(true)
		{
			_InputReady.Wait();
			
			// preferentially handle input port signals
			if(InputPortEvents.Count != 0)
			{
				PortEvent evt = InputPortEvents[0]; // pull from the front
				InputPortEvents.RemoveAt(0);		// and remove it from the list

				// only execute if there is a Forth execution token
				int xt = Ram.GetInt(IoInMapStart + evt.Port * ForthRAM.CellSize);
				if(xt != 0)
				{
					Push(evt.Value); // store the value
					Push(xt);		 // store the execution token
					CoreWords.Execute.Call();	 // execute the token
				}
			}

			// followed by timer timeouts
			else if(TimerEvents.Count != 0)
			{
				var id = TimerEvents.Dequeue();

				// only execute if Forth is still listening on this id
				var xt = Ram.GetInt(PeriodicStart + (id * 2 + 1) * ForthRAM.CellSize);
				if(xt != 0)
				{
					Push(xt);
					CoreWords.Execute.Call();
				}
				else
				{
					// not listening any longer. remove the timer.
					CallDeferred("_remove_timer", id);
				}
			}
			else
			{
				// no input events, must be terminal input line
				_OutputDone = false;
				InterpretTerminalLine();
				_OutputDone = true;
			}
		}
	}


	public void ResetBuffToIn()
	{
		// retrieve the address of the current buffer pointer
		CoreWords.ToIn.Call();
		// and set its contents to zero
		Ram.SetInt(Pop(), 0);
	}


	public static bool IsValidInt(string word, int radix = 10)
	{
		if(radix == 16)
		{
			return word.IsValidHexNumber();
		}
		return word.IsValidInt();
	}


	public static int ToInt(string word, int radix = 10)
	{
		if(radix == 16)
		{
			return word.HexToInt();
		}
		return word.ToInt();
	}


// Interpret the _terminal_pad content
	protected void InterpretTerminalLine()
	{
		// null terminate the string and convert to byte[]
		var bytes_input = (_TerminalPad + "\u0000").ToAsciiBuffer();
		_TerminalPad = "";
		_PadPosition = 0;
		// transfer to the RAM-based input buffer (accessible to the engine)
		for (int i = 0; i < bytes_input.Length; i++)
		{
			Ram.SetByte(BuffSourceStart + i, bytes_input[i]);
		}
		Push(BuffSourceStart);
		Push(bytes_input.Length);
		SourceId =  - 1;
		CoreWords.Evaluate.Call();
		Util.RprintTerm(" ok");
	}


// return echo text that refreshes the current edit
	protected string _RefreshEditText()
	{
		var echo = Forth.Terminal.CLRLINE
			+ Forth.Terminal.CR
			+ _TerminalPad
			+ Forth.Terminal.CR;
			
		foreach(int i in GD.Range(_PadPosition))
		{
			echo += Forth.Terminal.RIGHT;
		}
		return echo;
	}


	protected string _SelectBufferedCommand()
	{
		var selected_index = _BufferIndex;
		_TerminalPad = _TerminalBuffer[selected_index];
		_PadPosition = _TerminalPad.Length;
		return Forth.Terminal.CLRLINE + Forth.Terminal.CR + _TerminalPad;
	}
}
