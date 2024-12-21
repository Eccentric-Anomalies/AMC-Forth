using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Or : Forth.Words
    {
        public Or(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "OR";
            Description = "Return x3, the bit-wise inclusive or of x1 with x2.";
            StackEffect = "( x1 x2 - x3 )";
        }

        public override void Call()
        {
            Stack.Push(Stack.Pop() | Stack.Pop());
        }
    }
}
