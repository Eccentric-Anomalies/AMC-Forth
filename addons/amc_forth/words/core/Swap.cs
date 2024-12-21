using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Swap : Forth.Words
    {
        public Swap(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "SWAP";
            Description = "Exchange the top two items on the stack.";
            StackEffect = "( x1 x2 - x2 x1 )";
        }

        public override void Call()
        {
            var x1 = Stack.DataStack[Stack.DsP + 1];
            Stack.DataStack[Stack.DsP + 1] = Stack.DataStack[Stack.DsP];
            Stack.DataStack[Stack.DsP] = x1;
        }
    }
}
