using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class InAddr : Words
    {
        public InAddr(AMCForth forth, string wordset)
            : base(forth, wordset)
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
