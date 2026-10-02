using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class Out : Forth.Words
    {
        public Out(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "OUT";
            Description = "Save value x to I/O port p, possibly triggering Godot signal.";
            StackEffect = "( x p - )";
        }

        public override void Call()
        {
            Forth.CoreWords.Dup.Call();
            var port = Stack.Pop();
            Forth.CoreWords.Cells.Call();
            // offset in bytes
            Stack.Push(Map.IoOutStart);
            // address of output block
            Forth.CoreWords.Plus.Call();
            // output address
            Forth.CoreWords.Over.Call();
            // copy value
            var value = Stack.Pop();
            Forth.CoreWords.Store.Call();
			Forth.OutputPortMutex.Lock();
            if (Forth.OutputPortMap.ContainsKey(port))
            {
                CallDeferred("OutputEmitter", port, value);
            }
			Forth.OutputPortMutex.Unlock();
        }

        public void OutputEmitter(int port, int value)
        {
            // generate an output signal for every registered listener
			Forth.OutputPortMutex.Lock();
            foreach (AMCForth.OutputPortSignal signal in Forth.OutputPortMap[port])
            {
                signal.Owner.CallDeferred("emit_signal", signal.Signal, value);
            }
			Forth.OutputPortMutex.Unlock();
        }
    }
}
