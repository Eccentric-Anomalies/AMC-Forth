using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Dup : Forth.Words
    {
        public Dup(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DUP";
            Description = "Duplicate the top entry on the stack.";
            StackEffect = "( x - x x )";
        }

        public override void Call()
        {
            Stack.Push(Stack.DataStack[Stack.DsP]);
        }
    }
}
