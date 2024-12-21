using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Plus : Forth.Words
    {
        public Plus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "+";
            Description = "Add n1 to n2 leaving the sum n3.";
            StackEffect = "( n1 n2 - n3 )";
        }

        public override void Call()
        {
            Stack.Push(Stack.Pop() + Stack.Pop());
        }
    }
}
