using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class NotEqual : Forth.Words
    {
        public NotEqual(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "<>";
            Description = "Return true if and only if n1 is not equal to n2.";
            StackEffect = "( n1 n2 - flag )";
        }

        public override void Call()
        {
            var t = Stack.Pop();
            if (t != Stack.Pop())
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
