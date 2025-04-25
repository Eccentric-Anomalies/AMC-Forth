using System;
using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class PTimerX : Forth.Words
    {
        public PTimerX(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "P-TIMERX";
            Description =
                "Start a periodic timer with execution token xt, id i, and interval n "
                + "(msec), with xt to be executed on timer expiration. Does nothing if the id "
                + "is in use. Usage: <id> <msec> P-TIMER <name>. Note: Timeouts "
                + "less than 50 msec will suffer from long-term timing drift. Each timeout "
                + "50 msec or greater may be slightly inaccurate, but will average to "
                + "the correct period with no long-term drift.";
            StackEffect = "( xt i n - )";
        }

        public override void Call()
        {
            Forth.CoreWords.Swap.Call();
            // ( xt i n - xt n i )
            Forth.CoreWords.Dup.Call();
            // ( xt n i - xt n i i )
            var id = Stack.Pop();
            // ( xt n i i - xt n i )
            GetTimerAddress();
            // ( xt n i - xt n addr )
            Forth.CoreWords.Rot.Call();
            // ( xt n addr - n addr xt )
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
                    Forth.CallDeferred("StartPeriodicTimer", id, ms, xt);
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
