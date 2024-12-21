using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Negate : Forth.Words
    {
        public Negate(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "NEGATE";
            Description = "Change the sign of the top stack value.";
            StackEffect = "( n - -n )";
        }

        public override void Call()
        {
            Stack.DataStack[Stack.DsP] = -Stack.DataStack[Stack.DsP];
        }
    }
}
