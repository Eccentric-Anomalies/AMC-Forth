using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class False : Forth.Words
    {
        public False(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "FALSE";
            Description = "Return a false value: a single-cell with all bits clear.";
            StackEffect = "( - flag )";
        }

        public override void Call()
        {
            Stack.Push(AMCForth.False);
        }
    }
}
