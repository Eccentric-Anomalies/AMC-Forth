using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class ZeroEqual : Forth.Words
    {
        public ZeroEqual(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "0=";
            Description = "Return true if and only if n is equal to zero.";
            StackEffect = "( n - flag )";
        }

        public override void Call()
        {
            if (Stack.Pop() != 0)
            {
                Stack.Push(AMCForth.False);
            }
            else
            {
                Stack.Push(AMCForth.True);
            }
        }
    }
}
