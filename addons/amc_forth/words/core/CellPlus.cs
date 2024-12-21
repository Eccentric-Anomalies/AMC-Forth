using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class CellPlus : Forth.Words
    {
        public CellPlus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CELL+";
            Description = "Add the size in bytes of a cell to a_addr1, returning a_addr2.";
            StackEffect = "( a-addr1 - a-addr2 )";
        }

        public override void Call()
        {
            Stack.Push(RAM.CellSize);
            Forth.CoreWords.Plus.Call();
        }
    }
}
