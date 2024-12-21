using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DEquals : Forth.Words
    {
        public DEquals(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "D=";
            Description = "Return true if and only if d1 is equal to d2.";
            StackEffect = "( d1 d2 - flag )";
        }

        public override void Call()
        {
            var t = Stack.PopDint();
            if (Stack.PopDint() == t)
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
