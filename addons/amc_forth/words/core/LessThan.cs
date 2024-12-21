using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class LessThan : Forth.Words
    {
        public LessThan(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "<";
            Description = "Return true if and only if n1 is less than n2.";
            StackEffect = "( n1 n2 - flag )";
        }

        public override void Call()
        {
            var t = Stack.Pop();
            if (t > Stack.Pop())
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
