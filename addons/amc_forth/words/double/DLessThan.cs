using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DLessThan : Forth.Words
    {
        public DLessThan(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "D<";
            Description = "Return true if and only if d1 is less than d2.";
            StackEffect = "( d1 d2 - flag )";
        }

        public override void Call()
        {
            var t = Stack.PopDint();
            if (Stack.PopDint() < t)
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
