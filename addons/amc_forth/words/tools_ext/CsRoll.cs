using Godot;
using Godot.Collections;

namespace Forth.ToolsExt
{
    [GlobalClass]
    public partial class CsRoll : Forth.Words
    {
        public CsRoll(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CS-ROLL";
            Description = "Fetch the cell contents of the given address and display.";
            StackEffect = "( a-addr - )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.CfStackRoll(Stack.Pop());
        }
    }
}
