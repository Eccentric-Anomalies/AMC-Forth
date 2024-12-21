using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class ReverseV : Forth.Words
    {
        public ReverseV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "REVERSEV";
            Description = "Send REVERSE command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.REVERSE);
        }
    }
}
