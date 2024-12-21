using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class ToR : Forth.Words
    {
        public ToR(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = ">R";
            Description =
                "Remove the item on top of the data stack and put it on the return stack.";
            StackEffect = "(S: x - ) (R: - x )";
        }

        public override void Call()
        {
            Stack.RPush(Stack.Pop());
        }
    }
}
