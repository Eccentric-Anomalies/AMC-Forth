using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Depth : Forth.Words
    {
        public Depth(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "DEPTH";
            Description = "Return the number of single-cell values on the stack before execution.";
            StackEffect = "( - +n )";
        }

        public override void Call()
        {
            Stack.Push(Stack.DataStackSize - Stack.DsP);
        }
    }
}
