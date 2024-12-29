using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Aligned : Forth.Words
    {
        public Aligned(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "ALIGNED";
            Description = "Return a-addr, the first aligned address greater than or equal to addr.";
            StackEffect = "( addr - a-addr )";
        }

        public override void Call()
        {
            var a = Stack.Pop();
            if (a % RAM.CellSize != 0)
            {
                a = (a / RAM.CellSize + 1) * RAM.CellSize;
            }
            Stack.Push(a);
        }
    }
}
