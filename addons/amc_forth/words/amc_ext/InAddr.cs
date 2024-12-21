using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class InAddr : Words
    {
        public InAddr(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "IN-ADDR";
            Description = "Return memory addr from input port p.";
            StackEffect = "( p - addr )";
        }

        public override void Call()
        {
            Stack.Push(Stack.Pop() * RAM.CellSize + Map.IoInStart);
        }
    }
}
