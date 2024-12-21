using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class TwoDrop : Forth.Words
    {
        public TwoDrop(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2DROP";
            Description = "Remove the top pair of cells from the stack.";
            StackEffect = "( x1 x2 - )";
        }

        public override void Call()
        {
            Stack.Pop();
            Stack.Pop();
        }
    }
}
