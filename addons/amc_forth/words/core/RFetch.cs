using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class RFetch : Forth.Words
    {
        public RFetch(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "R@";
            Description =
                "Place a copy of the item on top of the return stack onto the data stack.";
            StackEffect = "(S: - x ) (R: x - x )";
        }

        public override void Call()
        {
            var t = Stack.RPop();
            Stack.Push(t);
            Stack.RPush(t);
        }
    }
}
