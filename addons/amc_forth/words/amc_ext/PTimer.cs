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
                + "less than 50 msec will suffer from long-term timing drift. Each timeout "
                + "50 msec or greater may be slightly inaccurate, but will average to "
                + "the correct period with no long-term drift. P-TIMER should not be used "
                + "inside a colon definition, unless <name> is provided following the "
                + "invocation of the definition.";
            StackEffect = "( 'name' i n - )";
        }

        public override void Call()
        {
            Forth.CoreWords.Tick.Call();
            // ( i n - i n xt )
            Forth.CoreWords.Rot.Call();
            // ( i n xt - n xt i )
            Forth.CoreWords.Rot.Call();
            // ( n xt i - xt i n )
            Forth.AMCExtWords.PTimerX.Call();
        }
    }
}
