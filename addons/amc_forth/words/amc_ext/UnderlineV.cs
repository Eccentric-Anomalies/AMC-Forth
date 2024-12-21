using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class UnderlineV : Forth.Words
    {
        public UnderlineV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "UNDERLINEV";
            Description = "Send UNDERLINE command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.UNDERLINE);
        }
    }
}
