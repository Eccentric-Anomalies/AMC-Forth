using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class LeftBracket : Forth.Words
    {
        public LeftBracket(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "[";
            Description = "Enter interpretation state.";
            StackEffect = "( - )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.State = false;
        }
    }
}
