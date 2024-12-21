using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class LowV : Forth.Words
    {
        public LowV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "LOWV";
            Description = "Send LOWINT (low intensity) command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.LOWINT);
        }
    }
}
