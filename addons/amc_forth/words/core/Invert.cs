using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Invert : Forth.Words
    {
        public Invert(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "INVERT";
            Description = "Invert all bits of x1, giving its logical inverse, x2.";
            StackEffect = "( x1 - x2 )";
        }

        public override void Call()
        {
            Stack.DataStack[Stack.DsP] = ~Stack.DataStack[Stack.DsP];
        }
    }
}
