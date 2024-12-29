using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class ZeroGreaterThan : Forth.Words
    {
        public ZeroGreaterThan(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "0>";
            Description = "Return true if and only if n is greater than zero.";
            StackEffect = "( n - flag )";
        }

        public override void Call()
        {
            if (Stack.Pop() > 0)
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
