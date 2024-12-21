using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class InvisibleV : Forth.Words
    {
        public InvisibleV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "INVISIBLEV";
            Description = "Send INVISIBLE command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.INVISIBLE);
        }
    }
}
