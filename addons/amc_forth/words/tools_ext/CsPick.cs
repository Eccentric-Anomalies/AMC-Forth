using Godot;
using Godot.Collections;

namespace Forth.ToolsExt
{
    [GlobalClass]
    public partial class CsPick : Forth.Words
    {
        public CsPick(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CS-PICK";
            Description = "Place copy of the uth CS entry on top of the CS stack.";
            StackEffect = "( i*x u - i*x x_u )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.CfStackPick(Stack.Pop());
        }
    }
}
