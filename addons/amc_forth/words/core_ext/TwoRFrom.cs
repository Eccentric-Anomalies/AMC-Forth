using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class TwoRFrom : Forth.Words
    {
        public TwoRFrom(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2R>";
            Description =
                "Pop the top two cells from the return stack and push them onto the "
                + "data stack.";
            StackEffect = "(S: - x1 x2 )  (R: x1 x2 - )";
        }

        public override void Call()
        {
            Stack.PushDint(Stack.RPopDint());
        }
    }
}
