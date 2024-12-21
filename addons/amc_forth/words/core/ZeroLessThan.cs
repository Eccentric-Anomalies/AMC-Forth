using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class ZeroLessThan : Forth.Words
    {
        public ZeroLessThan(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "0<";
            Description = "Return true if and only if n is less than zero.";
            StackEffect = "( n - flag )";
        }

        public override void Call()
        {
            if (Stack.Pop() < 0)
            {
                Stack.Push(AMCForth.True);
            }
            else
            {
                Stack.Push(AMCForth.False);
            }
        }
    }
}
