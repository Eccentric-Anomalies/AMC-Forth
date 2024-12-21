using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Drop : Forth.Words
    {
        public Drop(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DROP";
            Description = "Drop (remove) the top entry of the stack.";
            StackEffect = "( x - )";
        }

        public override void Call()
        {
            Stack.Pop();
        }
    }
}
