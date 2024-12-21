using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class TwoOver : Forth.Words
    {
        public TwoOver(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2OVER";
            Description = "Copy a cell pair x1 x2 to the top of the stack.";
            StackEffect = "( x1 x2 x3 x4 - x1 x2 x3 x4 x1 x2 )";
        }

        public override void Call()
        {
            var x2 = Stack.DataStack[Stack.DsP + 2];
            var x1 = Stack.DataStack[Stack.DsP + 3];
            Stack.Push(x1);
            Stack.Push(x2);
        }
    }
}
