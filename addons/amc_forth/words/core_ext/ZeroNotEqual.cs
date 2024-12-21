using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class ZeroNotEqual : Forth.Words
    {
        public ZeroNotEqual(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "0<>";
            Description = "Return true if and only if n is not equal to zero.";
            StackEffect = "( n - flag )";
        }

        public override void Call()
        {
            if (Stack.Pop() != 0)
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
