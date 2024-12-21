using Godot;

namespace Forth.Facility
{
    [GlobalClass]
    public partial class Page : Forth.Words
    {
        public Page(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "PAGE";
            Description =
                "On a CRT, clear the screen and reset cursor position to the upper left corner.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.CLRSCR);
            Stack.Push(1);
            Forth.CoreWords.Dup.Call();
            Forth.FacilityWords.AtXY.Call();
        }
    }
}
