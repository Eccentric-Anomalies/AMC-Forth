using System;
using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class PTimer : Forth.Words
    {
        public PTimer(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "P-TIMER";
            Description =
                "Start a periodic timer with id i, and interval n (msec) that "
                + "calls execution token given by <name>. Does nothing if the id "
                + "is in use. Usage: <id> <msec> P-TIMER <name>. Note: Timeouts "
                + "less than 10 msec will suffer from long-term timing drift. Each timeout "
                + "10 msec or greater may be slightly inaccurate, but will average to "
                + "the correct period with no long-term drift.";
            StackEffect = "( 'name' i n - )";
        }

        public override void Call()
        {
            Forth.CoreWords.Swap.Call();
            // ( i n - n i )
            Forth.CoreWords.Dup.Call();
            // ( n i - n i i )
            var id = Stack.Pop();
            // ( n i i - n i )
            GetTimerAddress();
            // ( n i - n addr )
            Forth.CoreWords.Tick.Call();
            // ( n addr - n addr xt )
            var xt = Stack.Pop();
            var addr = Stack.Pop();
            var ms = Stack.Pop();
            // ( - )
            try
            {
                if ((ms != 0) && (Forth.Ram.GetInt(addr) == 0))
                {
                    // only if non-zero and nothing already there
                    Forth.Ram.SetInt(addr, ms);
                    Forth.Ram.SetInt(addr + RAM.CellSize, xt);
                    Forth.StartPeriodicTimer(id, ms, xt);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Forth.Util.RprintTerm($" Timer ID out of range (maximum {Map.PeriodicTimerQty}).");
            }
        }

        public void GetTimerAddress()
        {
            // Utility to accept timer id and leave the start address of
            // its msec, xt pair
            // ( id - addr )
            Stack.Push(RAM.CellSize);
            Forth.CoreWords.TwoStar.Call();
            Forth.CoreWords.Star.Call();
            Stack.Push(Map.PeriodicStart);
            Forth.CoreWords.Plus.Call();
        }
    }
}
