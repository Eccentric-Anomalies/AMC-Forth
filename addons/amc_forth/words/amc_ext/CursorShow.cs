using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class CursorShow : Forth.Words
    {
        public CursorShow(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "CURSOR-SHOW";
            Description = "Send show cursor command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.CURSORSHOW);
        }
    }
}
