using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Unloop : Forth.Words
    {
        public Unloop(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "UNLOOP";
            Description = "Discard the loop parameters for the current nesting level.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.RPopDint();
        }
    }
}
