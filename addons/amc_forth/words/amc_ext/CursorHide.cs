using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class CursorHide : Forth.Words
    {
        public CursorHide(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "CURSOR-HIDE";
            Description = "Send cursor hide command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.CURSORHIDE);
        }
    }
}
