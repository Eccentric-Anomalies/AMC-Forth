using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Xor : Forth.Words
    {
        public Xor(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "XOR";
            Description = "Return x3, the bit-wise exclusive or of x1 with x2.";
            StackEffect = "( x1 x2 - x3 )";
        }

        public override void Call()
        {
            Stack.Push(Stack.Pop() ^ Stack.Pop());
        }
    }
}
