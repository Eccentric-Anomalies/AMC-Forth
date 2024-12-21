using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class BoldV : Forth.Words
    {
        public BoldV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "BOLDV";
            Description = "Send BOLD command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.BOLD);
        }
    }
}
