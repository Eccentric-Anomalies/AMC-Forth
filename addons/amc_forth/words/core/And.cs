using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class And : Forth.Words
    {
        public And(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "AND";
            Description = "Return x3, the bit-wise logical AND of x1 and x2.";
            StackEffect = "( x1 x2 - x3)";
        }

        public override void Call()
        {
            Stack.Push(Stack.Pop() & Stack.Pop());
        }
    }
}
