using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class TwoRFetch : Forth.Words
    {
        public TwoRFetch(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2R@";
            Description = "Push a copy of the top two return stack cells onto the data stack.";
            StackEffect = "(S: - x1 x2 ) (R: x1 x2 - x1 x2 )";
        }

        public override void Call()
        {
            var t = Stack.RPopDint();
            Stack.PushDint(t);
            Stack.RPushDint(t);
        }
    }
}
