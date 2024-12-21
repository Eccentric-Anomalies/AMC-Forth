using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Cells : Forth.Words
    {
        public Cells(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CELLS";
            Description = "Return n2, the size in bytes of n1 cells.";
            StackEffect = "( n1 - n2 )";
        }

        public override void Call()
        {
            Stack.Push(RAM.CellSize);
            Forth.CoreWords.Star.Call();
        }
    }
}
