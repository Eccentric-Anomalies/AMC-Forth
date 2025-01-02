using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Abs : Forth.Words
    {
        public Abs(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "ABS";
            Description = "Replace the top stack item with its absolute value.";
            StackEffect = "( n - +n )";
        }

        public override void Call()
        {
            // Absolute value of MAX-INT+1 is a noop
            if (Stack.DataStack[Stack.DsP] != int.MinValue)
            {
                Stack.DataStack[Stack.DsP] = System.Math.Abs(Stack.DataStack[Stack.DsP]);
            }
        }
    }
}
