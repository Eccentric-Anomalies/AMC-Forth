using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class BlinkV : Forth.Words
    {
        public BlinkV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "BLINKV";
            Description = "Send BLINK command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.BLINK);
        }
    }
}
