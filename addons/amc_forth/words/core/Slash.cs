using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Slash : Forth.Words
    {
        public Slash(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "/";
            Description = "Divide n1 by n2, leaving the quotient n3.";
            StackEffect = "( n1 n2 - n3 )";
        }

        public override void Call()
        {
            var d = Stack.Pop();
            Stack.Push(Stack.Pop() / d);
        }
    }
}
