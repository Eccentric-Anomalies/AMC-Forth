using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class RightBracket : Forth.Words
    {
        public RightBracket(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "]";
            Description = "Enter compilation state.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.State = true;
        }
    }
}
